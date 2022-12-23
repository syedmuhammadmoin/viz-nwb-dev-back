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
    public class QuotationComparativeSpecs : BaseSpecification<QuotationComparativeMaster>
    {
       
        public QuotationComparativeSpecs(List<DateTime?> docDate,
          List<DocumentStatus?> states, TransactionFormFilter filter, bool isTotalRecord)
             : base(x => (docDate.Count() > 0 ? docDate.Contains(x.QuotationComparativeDate) : true)
              && x.DocNo.Contains(filter.DocNo != null ? filter.DocNo : "")
          && (states.Count() > 0 ? states.Contains(x.State) : true))
        {
            if (!isTotalRecord)
            {
                var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
                ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
                ApplyOrderByDescending(i => i.Id);
                AddInclude(i => i.State);
            }
        }
        public QuotationComparativeSpecs(bool forEdit) 
        {
            if (forEdit)
            {
                AddInclude(i => i.QuotationComparativeLines);
                AddInclude(i => i.State);
            }
            else
            {
                AddInclude(i => i.State);
                AddInclude(i => i.QuotationComparativeLines);
            }
        }


    }
}
