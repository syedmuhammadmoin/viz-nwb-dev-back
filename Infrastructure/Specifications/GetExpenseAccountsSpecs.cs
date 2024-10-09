using Domain.Constants;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class GetExpenseAccountsSpecs : BaseSpecification<Level4>
    {
        public GetExpenseAccountsSpecs(int orgId)
              : base(x => x.Level1_id == $"{FinanceAccountTypes.Expenses}-{orgId}")
        {
        }
    }
}
