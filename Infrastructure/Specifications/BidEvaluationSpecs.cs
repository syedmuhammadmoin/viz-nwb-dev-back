using Application.Contracts.Filters;
using Domain.Constants;
using Domain.Entities;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class BidEvaluationSpecs : BaseSpecification<BidEvaluationMaster>
    {
        public BidEvaluationSpecs(List<DateTime?> OpeningDate,
        List<DocumentStatus?> states, TransactionFormFilter filter, bool isTotalRecord) : base(x => 
        (OpeningDate.Count() > 0 ? OpeningDate.Contains(x.DateOfOpeningBid) : true)
         && x.DocNo.Contains(filter.DocNo != null ? filter.DocNo : "")
        && (states.Count() > 0 ? states.Contains(x.State) : true))
        {
            if (!isTotalRecord)
            {
                var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
                ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
                ApplyOrderByDescending(i => i.Id);

            }
        }
        public BidEvaluationSpecs(bool forEdit)
        {
            if (forEdit)
            {
                AddInclude(i => i.BidEvaluationLines);
            }
            else
            {
                AddInclude(i => i.BidEvaluationLines);
            }
        }
    }
}
