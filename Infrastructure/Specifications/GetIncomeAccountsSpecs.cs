using Domain.Constants;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class GetIncomeAccountsSpecs : BaseSpecification<Level4>
    {
        public GetIncomeAccountsSpecs(int orgId)
              : base(x => x.Level1_id == $"{FinanceAccountTypes.Income}-{orgId}")
        {
        }
    }
}
