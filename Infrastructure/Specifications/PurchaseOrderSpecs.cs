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
    public class PurchaseOrderSpecs : BaseSpecification<PurchaseOrderMaster>
    {
        public PurchaseOrderSpecs(List<DateTime?> docDate, List<DateTime?> dueDate,
            List<DocumentStatus?> states, TransactionFormFilter filter, bool isTotalRecord) : base(x => (docDate.Count() > 0 ? docDate.Contains(x.PODate) : true)
            && (dueDate.Count() > 0 ? dueDate.Contains(x.DueDate) : true)
            && x.DocNo.Contains(filter.DocNo != null ? filter.DocNo : "")
            && x.Vendor.Name.Contains(filter.BusinessPartner != null ? filter.BusinessPartner : "")	
			&& x.PODate.Month == (filter.Month != null ? Convert.ToInt32(filter.Month) : x.PODate.Month)
		    && x.PODate.Year == (filter.Year != null ? Convert.ToInt32(filter.Year) : x.PODate.Year)
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
                AddInclude("PurchaseOrderLines.Account");
                AddInclude("PurchaseOrderLines.Item");
                AddInclude("PurchaseOrderLines.Warehouse");
            }
        }

        public PurchaseOrderSpecs(bool forEdit)
        {
            if (forEdit)
            {
                AddInclude(i => i.PurchaseOrderLines);
                AddInclude(i => i.Status);
            }
            else
            {
                AddInclude(i => i.Campus);
                AddInclude(i => i.Status);
                AddInclude(i => i.Vendor);
                AddInclude("PurchaseOrderLines.Account");
                AddInclude("PurchaseOrderLines.Item");
                AddInclude("PurchaseOrderLines.Warehouse");
            }
        }
        
        public PurchaseOrderSpecs()
            : base(x => x.Status.State != DocumentStatus.Paid)
        {
            AddInclude(i => i.PurchaseOrderLines);
        }
        
        public PurchaseOrderSpecs(int id) : base(x => x.Id == id)
        {
            AddInclude(i => i.PurchaseOrderLines);
        }

        public PurchaseOrderSpecs(string workflow)
            : base(e => (e.Status.State != DocumentStatus.Unpaid && e.Status.State != DocumentStatus.Partial && e.Status.State != DocumentStatus.Paid && e.Status.State != DocumentStatus.Draft && e.Status.State != DocumentStatus.Cancelled))
        {
            AddInclude(i => i.Status);
        }

        public PurchaseOrderSpecs(int requisitionId, bool isReff)
         : base(x => x.RequisitionId == requisitionId)
        {
            AddInclude(i => i.PurchaseOrderLines);
            AddInclude("PurchaseOrderLines.Item");
        }
    }
}
