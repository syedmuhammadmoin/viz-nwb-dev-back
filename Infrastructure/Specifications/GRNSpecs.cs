using Application.Contracts.Filters;
using Domain.Constants;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class GRNSpecs : BaseSpecification<GRNMaster>
    {
        public GRNSpecs(List<DateTime?> docDate,
            List<DocumentStatus?> states, TransactionFormFilter filter, bool isTotalRecord)
            : base(x => (docDate.Count() > 0 ? docDate.Contains(x.GrnDate) : true)
            && x.DocNo.Contains(filter.DocNo != null ? filter.DocNo : "")
            && x.Vendor.Name.Contains(filter.BusinessPartner != null ? filter.BusinessPartner : "")
            && (states.Count() > 0 ? states.Contains(x.Status.State) : true))
        {
            if (!isTotalRecord)
            {
                var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
                ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
                ApplyOrderByDescending(i => i.Id);
                AddInclude(i => i.Campus);
                AddInclude(i => i.Status);
                AddInclude(i => i.Vendor);
                AddInclude(i => i.PurchaseOrder);
                AddInclude("GRNLines.Item");
                AddInclude("GRNLines.Warehouse");
            }
        }

        public GRNSpecs(bool forEdit)
        {
            if (forEdit)
            {
                AddInclude(i => i.GRNLines);
                AddInclude(i => i.Status);
            }
            else
            {
                AddInclude(i => i.Campus);
                AddInclude(i => i.Status);
                AddInclude(i => i.Vendor);
                AddInclude(i => i.PurchaseOrder);
                AddInclude("GRNLines.Item");
                AddInclude("GRNLines.Warehouse");
            }
        }
        public GRNSpecs()
            : base(x => x.Status.State != DocumentStatus.Paid)
        {
            AddInclude(i => i.GRNLines);
        }
        public GRNSpecs(int id) : base(x => x.Id == id)
        {
            AddInclude(i => i.GRNLines);
        }
    }
}
