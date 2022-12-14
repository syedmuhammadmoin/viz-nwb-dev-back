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
    public class BidEvaluationSpecs : BaseSpecification<BidEvaluationMaster>
    {
        public BidEvaluationSpecs(List<DateTime?> OpeningDate, List<DateTime?> CloseingDate,
            List<DocumentStatus?> states, TransactionFormFilter filter, bool isTotalRecord)
        {
        }

        public BidEvaluationSpecs(bool forEdit)
        {
            if (forEdit)
            {
                AddInclude(i => i.BidEvaluationLines);
            }
            else
            {
           
            }
        }
    }
}
