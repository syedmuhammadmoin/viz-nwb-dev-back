using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class TransactionReconcile : BaseEntity<int>
    {
        public int PaymentLedgerId { get; private set; }
        [ForeignKey("PaymentLedgerId")]
        public RecordLedger PaymentLedger { get; private set; }
        public int DocumentLegderId { get; private set; }
        [ForeignKey("DocumentLegderId")]
        public RecordLedger DocumentLedger { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; private set; }

        protected TransactionReconcile()
        {
        }

        public TransactionReconcile(int paymentLedgerId, int documentLegderId, decimal amount)
        {
            PaymentLedgerId = paymentLedgerId;
            DocumentLegderId = documentLegderId;
            Amount = amount;
        }
    }
}
