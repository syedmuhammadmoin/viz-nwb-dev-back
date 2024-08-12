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
    public class PettyCashMaster : BaseEntity<int>
    {
        [MaxLength(20)]
        public string DocNo { get; private set; }
        public DateTime Date { get; private set; }
        [MaxLength(500)]
        public string Description { get; private set; }
        public string AccountId { get; private set; }
        [ForeignKey("AccountId")]
        public Level4 Account { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalDebit { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalCredit { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal OpeningBalance { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal ClosingBalance { get; private set; }
        public int StatusId { get; private set; }
        [ForeignKey("StatusId")]
        public WorkFlowStatus Status { get; private set; }
        public int? CampusId { get; private set; }
        [ForeignKey("CampusId")]
        public Campus Campus { get; private set; }
        public int? TransactionId { get; private set; }
        [ForeignKey("TransactionId")]
        public Transactions Transactions { get; private set; }
        public virtual List<PettyCashLines> PettyCashLines { get; private set; }

        protected PettyCashMaster()
        {
        }

        public void SetStatus(int statusId)
        {
            StatusId = statusId;
        }

        public void SetTransactionId(int transactionId)
        {
            TransactionId = transactionId;
        }

        public void CreateDocNo()
        {
            //Creating doc no..
            DocNo = "PTC-" + String.Format("{0:000}", Id);
        }
    }
}
