﻿using Domain.Base;
using Domain.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class QuotationComparativeMaster : BaseEntity<int>
    {
        [MaxLength(30)]
        public string DocNo { get; private set; }
        public DateTime QuotationComparativeDate { get; private set; }

        public int RequsisitionId { get; private set; }
        [ForeignKey("RequsisitionId")]
        public RequisitionMaster Requisition { get; private set; }
        
        [MaxLength(200)] 
        public string Remarks { get; private set; }
        public DocumentStatus State { get; private set; }
        
        public virtual List<QuotationMaster> Quotations { get; private set; }

        protected QuotationComparativeMaster()
        {
        }
        public void setStatus(DocumentStatus status)
        {
            State = status;
        }

        public void CreateDocNo()
        {
            DocNo = "QC-" + String.Format("{0:000}", Id);
        }
    }
}
