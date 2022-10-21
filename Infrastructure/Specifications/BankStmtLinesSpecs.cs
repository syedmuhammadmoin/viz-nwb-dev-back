using Domain.Constants;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class BankStmtLinesSpecs : BaseSpecification<BankStmtLines>
    {
        public BankStmtLinesSpecs() : base(l => (l.BankReconStatus != DocumentStatus.Reconciled))
        {
        }
        
        public BankStmtLinesSpecs(int bankAcctId) 
            : base(x =>
            (x.BankStmtMaster.BankAccountId == bankAcctId)
            && (x.BankReconStatus == DocumentStatus.Unreconciled || x.BankReconStatus == DocumentStatus.Partial)
            )
        {
            AddInclude(i => i.BankStmtMaster);
        }
    }
}
