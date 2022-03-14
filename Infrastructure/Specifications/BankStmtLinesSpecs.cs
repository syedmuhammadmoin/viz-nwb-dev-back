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
        public BankStmtLinesSpecs() : base(l => (l.BankReconStatus != ReconStatus.Reconciled))
        {
        }
    }
}
