using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Constants;

namespace Domain.Entities
{
    public class CallForQuotationMaster : BaseEntity<int>
    {
        public int VendorId { get; private set; }
        [ForeignKey("VendorId")]
        public BusinessPartner Vendor { get; private set; }
        [MaxLength(100)]
        public string DocNo { get; private set; }
        [MaxLength(500)]
        public string Description { get; private set; }
        public DateTime CallForQuotationDate { get; private set; }
        public DocumentStatus State { get; set; }
        public virtual List<CallForQuotationLines> CallForQuotationLines { get; private set; }

        protected CallForQuotationMaster()
        {
        }
        public void setStatus(DocumentStatus statusId)
        {
            State = statusId;
        }
        public void CreateDocNo()
        {
            DocNo = "CFQ-" + String.Format("{0:000}", Id);
        }
    }
}
