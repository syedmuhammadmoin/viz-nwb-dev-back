using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class PurchaseOrderMaster : BaseEntity<int>
    {
        public int VendorId { get; private set; }
        [ForeignKey("VendorId")]
        public BusinessPartner Vendor { get; private set; }
        [MaxLength(20)]
        public string DocNo { get; private set; }
        public DateTime PODate { get; private set; }
        public DateTime DueDate { get; private set; }
        [MaxLength(20)]
        public string Contact { get; private set; }
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
        public virtual List<PurchaseOrderLines> PurchaseOrderLines { get; private set; }

        protected PurchaseOrderMaster()
        {

        }
        public void setStatus(int statusId)
        {
            StatusId = statusId;
        }
        public void CreateDocNo()
        {
            //Creating doc no..
            DocNo = "PO-" + String.Format("{0:000}", Id);
        }
    }
}
