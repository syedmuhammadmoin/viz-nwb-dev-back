using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class TransactionReconSpecs : BaseSpecification<TransactionReconcile>
    {
        public TransactionReconSpecs(int ledgerId, bool isPaymentId) 
            : base(isPaymentId ? p => p.PaymentLedgerId == ledgerId
            : p => p.DocumentLegderId == ledgerId)
        {
            AddInclude("PaymentLedger.Transactions");
            AddInclude("DocumentLedger.Transactions");
            ApplyAsNoTracking();
        }
    }
}
