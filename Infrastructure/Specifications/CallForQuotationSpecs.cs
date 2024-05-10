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
    public class CallForQuotationSpecs : BaseSpecification<CallForQuotationMaster>
    {
      
        public CallForQuotationSpecs(List<DateTime?> docDate,
          List<DocumentStatus?> states, TransactionFormFilter filter, bool isTotalRecord)
          : base(x => (docDate.Count() > 0 ? docDate.Contains(x.CallForQuotationDate
              ) : true)
             && (x.Description.Contains(filter.Description != null ? filter.Description : "")) && x.DocNo.Contains(filter.DocNo != null ? filter.DocNo : "")
          && x.Vendor.Name.Contains(filter.BusinessPartner != null ? filter.BusinessPartner : "")
          && (states.Count() > 0 ? states.Contains(x.State) : true))
        {
            if (!isTotalRecord)
            {
                var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
                ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
                ApplyOrderByDescending(i => i.Id);
                AddInclude(i => i.Vendor);
            }
        }
        public CallForQuotationSpecs(bool forEdit)
        {
            if (forEdit)
            {
                AddInclude(i => i.CallForQuotationLines);
            }
            else
            {
                AddInclude(i => i.Vendor);
                AddInclude(i => i.CallForQuotationLines);
                AddInclude("CallForQuotationLines.Item");
            }
        }


    }
}
