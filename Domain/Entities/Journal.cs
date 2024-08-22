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
        public Types Type { get; private set; }
        public string BankAcountId { get; private set; }
        [ForeignKey("BankAccountId")]
        public BankAccount BankAccount { get; private set; }
        [MaxLength(50)]
        public string BankName { get; private set; }
        [MaxLength(50)]
        public string AccountNumber { get; private set; }
        public string SuspenseAccount { get; private set; }
        public string ProfitAccount { get; private set; }
        public string LossAccount { get; private set; }
        public string CashAccount { get; private set; }
        public int OrganizationId { get; set; }
       




    }
}
