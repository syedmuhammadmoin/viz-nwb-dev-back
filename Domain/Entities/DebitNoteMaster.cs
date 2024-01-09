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
    public class DebitNoteMaster : BaseEntity<int>
    {
        public int VendorId { get; private set; }
        [ForeignKey("VendorId")]
        public BusinessPartner Vendor { get; private set; }
        [MaxLength(20)]
        public string DocNo { get; private set; }
        public DateTime NoteDate { get; private set; }
        public Guid PayableAccountId { get; private set; }
        [ForeignKey("PayableAccountId")]
        public Level4 PayableAccount { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalBeforeTax { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Tax { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalTax { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal OtherTax { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; private set; }
        public int StatusId { get; set; }
        [ForeignKey("StatusId")]
        public WorkFlowStatus Status { get; set; }
        public int? CampusId { get; private set; }
        [ForeignKey("CampusId")]
        public Campus Campus { get; private set; }
        public int? TransactionId { get; private set; }
        [ForeignKey("TransactionId")]
        public Transactions Transactions { get; private set; }
        public int? LedgerId { get; private set; }
        public int? DocumentLedgerId { get; private set; }
        public virtual List<DebitNoteLines> DebitNoteLines { get; private set; }

        protected DebitNoteMaster()
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
        public void SetLedgerId(int ledgerId)
        {
            LedgerId = ledgerId;
        }

        public void SetPayableAccountId(Guid payableAccountId)
        {
            PayableAccountId = payableAccountId;
        }
        public void CreateDocNo()
        {
            //Creating doc no..
            DocNo = "DBN-" + String.Format("{0:000}", Id);
        }
    }
}
