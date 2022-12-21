
using Application.Contracts.Filters;
using Domain.Constants;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class QuotationSpecs : BaseSpecification<QuotationMaster>
    {
        public QuotationSpecs(List<DateTime?> docDate,
          List<DocumentStatus?> states, TransactionFormFilter filter, bool isTotalRecord)
          : base(x => (docDate.Count() > 0 ? docDate.Contains(x.QuotationDate) : true)
             && x.DocNo.Contains(filter.DocNo != null ? filter.DocNo : "")
          && x.Vendor.Name.Contains(filter.BusinessPartner != null ? filter.BusinessPartner : "")
          && (states.Count() > 0 ? states.Contains(x.Status.State) : true))
        {
            if (!isTotalRecord)
            {
                var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
                ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
                ApplyOrderByDescending(i => i.Id);
                AddInclude(i => i.Status);
                AddInclude(i => i.Vendor);
            }
        }



        public QuotationSpecs(bool forEdit)
        {
            if (forEdit)
            {
                AddInclude(i => i.QuotationLines);
                AddInclude(i => i.Status);
            }
            else
            {
                AddInclude(i => i.Status);
                AddInclude(i => i.Vendor);
                AddInclude(i => i.QuotationLines);
            }
        }
        public QuotationSpecs() : base(x => x.Status.State != DocumentStatus.Paid)
        {
            AddInclude(i => i.QuotationLines);
        }

        public QuotationSpecs(string workflow)
         : base(e => (e.Status.State != DocumentStatus.Unpaid && e.Status.State != DocumentStatus.Partial && e.Status.State != DocumentStatus.Paid && e.Status.State != DocumentStatus.Draft && e.Status.State != DocumentStatus.Cancelled))
        {
            AddInclude(i => i.Status);
        }

        public QuotationSpecs( int requisitionNo)
            : base(x => x.RequisitionId == requisitionNo)
        {
          
                ApplyOrderByDescending(i => i.Id);
                AddInclude(i => i.QuotationLines);
                AddInclude(i => i.Status);
          

        }
    }
}
