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
    public class QuotationComparativeMaster : BaseEntity<int>
    {
        public int RequsisitionId { get; private set; }
        [ForeignKey("MasterId")]
        public RequisitionMaster Requisition { get; private set; }
        [MaxLength(30)]
        public string DocNo { get; private set; }
        public DateTime QuotationComparativeDate { get; private set; }
        public DocumentStatus State { get; private set; }
        public string Remarks { get; private set; }

        public virtual List<QuotationComparativeLines> QuotationComparativeLines { get; private set; }

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
