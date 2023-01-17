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
    public class InvoiceMaster : BaseEntity<int>
    {
        public int CustomerId { get; private set; }
        [ForeignKey("CustomerId")]
        public BusinessPartner Customer { get; private set; }
        [MaxLength(20)]
        public string DocNo { get; private set; }
        public DateTime InvoiceDate { get; private set; }
        public DateTime DueDate { get; private set; }
        [MaxLength(20)]
        public string Contact { get; private set; }
        public Guid ReceivableAccountId { get; private set; }
        [ForeignKey("ReceivableAccountId")]
        public Level4 ReceivableAccount { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalBeforeTax { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalTax { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; private set; }
        public int StatusId { get; private set; }
        [ForeignKey("StatusId")]
        public WorkFlowStatus Status { get; private set; }
        public int CampusId { get; private set; }
        [ForeignKey("CampusId")]
        public Campus Campus { get; private set; }
        public int? LedgerId { get; private set; }
        public int? TransactionId { get; private set; }
        [ForeignKey("TransactionId")]
        public Transactions Transactions { get; private set; }
        public virtual List<InvoiceLines> InvoiceLines { get; private set; }
        
        
        protected InvoiceMaster()
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
        public void SetReceivableAccount(Guid receivableAccountId)
        {
            ReceivableAccountId = receivableAccountId;
        }
        public void CreateDocNo()
        {
            //Creating doc no..
            DocNo = "INV-" + String.Format("{0:000}", Id);
        }
    }
}
