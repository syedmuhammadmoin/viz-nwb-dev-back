using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class BankReconciliation : BaseEntity<int>
    {
        public int BankStmtId { get; private set; }
        [ForeignKey("BankStmtId")]
        public BankStmtLines BankStmt { get; private set; }
        public int PaymentId { get; private set; }
        [ForeignKey("PaymentId")]
        public Payment Payment { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; private set; }

        protected BankReconciliation()
        {

        }
    }
}
