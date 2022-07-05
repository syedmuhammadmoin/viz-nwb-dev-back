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
    public class IssuanceMaster : BaseEntity<int>
    {

        public int EmployeeId { get; private set; }
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; private set; }
        public DateTime IssuanceDate { get; private set; }
        [MaxLength(20)]
        public string DocNo { get; private set; }
        public int? RequisitionId { get; private set; }
        [ForeignKey("RequisitionId")]
        public RequisitionMaster Requisition { get; private set; }
        public int CampusId { get; private set; }
        [ForeignKey("CampusId")]
        public Campus Campus { get; private set; }
        public int StatusId { get; private set; }
        [ForeignKey("StatusId")]
        public WorkFlowStatus Status { get; private set; }
        public virtual List<IssuanceLines> IssuanceLines { get; private set; }
        public void setStatus(int statusId)
        {
            StatusId = statusId;
        }
        public void CreateDocNo()
        {
            //Creating doc no..
            DocNo = "ISU-" + String.Format("{0:000}", Id);
        }
        protected IssuanceMaster()
        {
        }
    }
}
