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
    public class BillMaster : BaseEntity<int>
    {
        public int VendorId { get; private set; }
        [ForeignKey("VendorId")]
        public BusinessPartner Vendor { get; private set; }
        [MaxLength(50)]
        public string DocNo { get; private set; }
        public DateTime BillDate { get; private set; }
        public DateTime DueDate { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalBeforeTax { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalTax { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; private set; }
        [MaxLength(20)]
        public string Contact { get; private set; }
        public int StatusId { get; private set; }
        [ForeignKey("StatusId")]
        public WorkFlowStatus Status { get; private set; }
        public int? TransactionId { get; private set; }
        [ForeignKey("TransactionId")]
        public Transactions Transactions { get; private set; }
        public virtual List<BillLines> BillLines { get; private set; }

        protected BillMaster()
        {

        }
        public void setStatus(int statusId)
        {
            StatusId = statusId;
        }

        public void setTransactionId(int transactionId)
        {
            TransactionId = transactionId;
        }

        public void CreateDocNo()
        {
            //Creating doc no..
            DocNo = "BILL-" + String.Format("{0:000}", Id);
        }
    }
}
