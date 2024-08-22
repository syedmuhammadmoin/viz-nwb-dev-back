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
    public class BankAccount : BaseEntity<int>
    {
        [MaxLength(20)]
        public string DocNo { get; private set; }
        [MaxLength(50)]
        public string AccountNumber { get; private set; }
        [MaxLength(50)]
        public string IBAN { get; private set; }
        [MaxLength(50)]
        public string AccountTitle { get; private set; }
        public BankAccountType BankAccountType { get; private set; }
        [MaxLength(50)]
        public string BankName { get; private set; }
        [MaxLength(50)]
        public string Branch { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal OpeningBalance { get; private set; }
        public DateTime OpeningBalanceDate { get; private set; }
        [MaxLength(200)]
        public string Purpose { get; private set; }
        public string ChAccountId { get; private set; }
        [ForeignKey("ChAccountId")]
        public Level4 ChAccount { get; private set; }
        public string ClearingAccountId { get; private set; }
        [ForeignKey("ClearingAccountId")]
        public Level4 ClearingAccount { get; private set; }
        public int? CampusId { get; private set; }
        [ForeignKey("CampusId")]
        public Campus Campus { get; private set; }
        public int TransactionId { get; private set; }
        [ForeignKey("TransactionId")]
        public Transactions Transactions { get; private set; }
        public Journal Journal { get; private set; }

        protected BankAccount()
        {

        }
        public void SetChAccountId(string chAccountId)
        {
            ChAccountId = chAccountId;
        }
        public void SetClAccountId(string clAccountId)
        {
            ClearingAccountId = clAccountId;
        }

        public void setTransactionId(int transactionId)
        {
            TransactionId = transactionId;
        }

        public void CreateDocNo()
        {
            //Creating doc no..
            DocNo = "BNK-" + String.Format("{0:000}", Id);
        }
    }
}
