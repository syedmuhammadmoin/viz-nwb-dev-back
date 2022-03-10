using Domain.Base;
using Domain.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class BankStmtMaster : BaseEntity<int>
    {
        public int BankAccountId { get; private set; }
        [ForeignKey("BankAccountId")]
        public BankAccount BankAccount { get; private set; }
        [MaxLength(500)]
        public string Description { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal OpeningBalance { get; private set; }
        public virtual List<BankStmtLines> BankStmtLines { get; private set; }

        protected BankStmtMaster()
        {

        }

    }
}
