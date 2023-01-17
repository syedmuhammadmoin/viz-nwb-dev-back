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
    public class GRNMaster : BaseEntity<int>
    {
        public int VendorId { get; private set; }
        [ForeignKey("VendorId")]
        public virtual BusinessPartner Vendor { get; private set; }
        [MaxLength(20)]
        public string DocNo { get; private set; }
        public DateTime GrnDate { get; private set; }
        [MaxLength(20)]
        public string Contact { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalBeforeTax { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalTax { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; private set; }
        public int CampusId { get; private set; }
        [ForeignKey("CampusId")]
        public Campus Campus { get; private set; }
        public int PurchaseOrderId { get; private set; }
        [ForeignKey("PurchaseOrderId")]
        public PurchaseOrderMaster PurchaseOrder { get; private set; }
        public int StatusId { get; private set; }
        [ForeignKey("StatusId")]
        public WorkFlowStatus Status { get; private set; }
        public virtual List<GRNLines> GRNLines { get; private set; }

        protected GRNMaster()
        {

        }
        public void SetStatus(int statusId)
        {
            StatusId = statusId;
        }

        public void SetPurchaseOrderId(int purchaseId)
        {
            PurchaseOrderId = purchaseId;
        }
        public void CreateDocNo()
        {
            //Creating doc no..
            DocNo = "GRN-" + String.Format("{0:000}", Id);
        }
    }
}
