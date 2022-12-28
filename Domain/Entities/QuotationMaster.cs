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
    public class QuotationMaster : BaseEntity<int>
    {
        [MaxLength(30)]
        public string DocNo { get; private set; }
        public DateTime QuotationDate { get; private set; }

        public int VendorId { get; private set; }
        [ForeignKey("VendorId")]
        public BusinessPartner Vendor { get; private set; }
        
        [MaxLength(100)]
        public string Timeframe { get; private set; }

        public int? RequisitionId { get; private set; }
        
        public int? QuotationComparativeId { get; private set; }
        [ForeignKey("QuotationComparativeId")]
        public QuotationComparativeMaster QuotationComparativeMaster { get; private set; }
        
        public int StatusId { get; private set; }
        [ForeignKey("StatusId")]
        public WorkFlowStatus Status { get; private set; }
        
        public virtual List<QuotationLines> QuotationLines { get; private set; }

        protected QuotationMaster()
        {
        }

        public void setStatus(int statusId)
        {
            StatusId = statusId;
        }

        public void CreateDocNo()
        {
            DocNo = "QUOTE-" + String.Format("{0:000}", Id);
        }
        public void UpdateQuotationComparativeMasterId(int? id)
        {
            QuotationComparativeId = id;
        }
    }
}
