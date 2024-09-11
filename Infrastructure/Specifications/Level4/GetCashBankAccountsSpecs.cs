using Domain.Constants;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class GetCashBankAccountsSpecs : BaseSpecification<Level4>
    {
        public GetCashBankAccountsSpecs(int orgId)
              : base(x => x.Level3_id == $"{FinanceLevel3Accounts.AssetsBankCash}-{orgId}")
        {
        }
    }
}
