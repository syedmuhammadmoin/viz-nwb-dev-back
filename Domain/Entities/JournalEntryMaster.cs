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
    public class JournalEntryMaster : BaseEntity<int>
    {
        [MaxLength(20)]
        public string DocNo { get; private set; }
        public DateTime Date { get; private set; }
        [MaxLength(500)]
        public string Description { get; private set; }
       
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalDebit { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalCredit { get; private set; }
        public int StatusId { get; private set; }
        [ForeignKey("StatusId")]
        public WorkFlowStatus Status { get; private set; }
        public int? CampusId { get; private set; }
        [ForeignKey("CampusId")]
        public Campus Campus { get; private set; }
        public int? TransactionId { get; private set; }
        [ForeignKey("TransactionId")]
        public Transactions Transactions { get; private set; }
        public virtual List<JournalEntryLines> JournalEntryLines { get; private set; }

        protected JournalEntryMaster()
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
            DocNo = "JV-" + String.Format("{0:000}", Id);
        }
    }
}
