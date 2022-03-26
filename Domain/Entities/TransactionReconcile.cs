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
        public int PaymentTransactionId { get; private set; }
        [ForeignKey("PaymentTransactionId")]
        public Transactions PaymentTransaction { get; private set; }
        public int DocumentTransactionId { get; private set; }
        [ForeignKey("DocumentTransactionId")]
        public Transactions DocumentTransaction { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; private set; }

        protected TransactionReconcile()
        {
        }
    }
}
