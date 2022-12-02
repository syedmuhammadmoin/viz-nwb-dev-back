using Domain.Base;
using Domain.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class PayrollTransactionLines : BaseEntity<int>
    {
        public int PayrollItemId { get;private set; }
        [ForeignKey("PayrollItemId")]
        public PayrollItem PayrollItem { get;private set; }
        public PayrollType PayrollType { get;private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Value { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; private set; }
        public Guid AccountId { get;private set; }
        [ForeignKey("AccountId")]
        public Level4 Account { get;private set; }
        public int MasterId { get;private set; }
        [ForeignKey("MasterId")]
        public PayrollTransactionMaster PayrollTransactionMaster { get;private set; }

        protected PayrollTransactionLines()
        {
        }

        public PayrollTransactionLines(int payrollItemId, PayrollType payrollType, decimal value, decimal amount, Guid accountId)
        {
            PayrollItemId = payrollItemId;
            PayrollType = payrollType;
            Value = value;
            Amount = amount;
            AccountId = accountId;
        }
    }
}
