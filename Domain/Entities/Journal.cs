using Domain.Base;
using Domain.Constants;
using Domain.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{

    public class Journal :  BaseEntity<int> ,IMustHaveTenant
    {
        public string Name { get; private set; }
        public JournalTypes JournalType { get; private set; }
        public int? BankAccountId { get; private set; }
        public string BankNameId { get; private set; }
        public string AccountNumberId { get; private set; }
        public string SuspenseAccountId { get; private set; }
        public string ProfitAccountId { get; private set; }
        public string LossAccountId { get; private set; }
        public string CashAccountId { get; private set; }
        public string DefaultAccountId { get; private set; }

        [ForeignKey("BankAccountId")]
        public BankAccount BankAccount { get; private set; }
        [MaxLength(50)]
        [ForeignKey("BankNameId")]
        public Level4 BankName { get; private set; }
        [MaxLength(50)]
        [ForeignKey("AccountNumberId")]
        public Level4 AccountNumber { get; private set; }
        [ForeignKey("SuspenseAccountId")]
        public Level4 SuspenseAccount { get; private set; }
        [ForeignKey("ProfitAccountId")]
        public Level4 ProfitAccount { get; private set; }
        [ForeignKey("LossAccountId")]
        public Level4 LossAccount { get; private set; }
        [ForeignKey("CashAccountId")]
        public Level4 CashAccount { get; private set; }
        [ForeignKey("DefaultAccountId")]
        public Level4 DefaultAccount { get; private set; }
        public int OrganizationId { get; set; }      

    }
}
