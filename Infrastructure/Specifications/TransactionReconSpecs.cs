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
        public TransactionReconSpecs(int transactionId, bool isPaymentId) 
            : base(isPaymentId ? p => p.PaymentTransactionId == transactionId
            : p => p.DocumentTransactionId == transactionId) {}
    }
}
