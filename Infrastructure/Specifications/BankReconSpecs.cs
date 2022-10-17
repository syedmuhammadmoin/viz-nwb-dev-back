using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class BankReconSpecs : BaseSpecification<BankReconciliation>
    {
        public BankReconSpecs(int id, bool isPaymentId)
            : base(isPaymentId ? p => p.PaymentId == id
            : p => p.BankStmtId == id)
        { 
        ApplyAsNoTracking();
        }
    }
}
