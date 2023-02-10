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
    public class IssuanceReturnMaster : BaseEntity<int>
    {
        public int EmployeeId { get; private set; }
        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; private set; }
        [MaxLength(20)]
        public string DocNo { get; private set; }
        public DateTime IssuanceReturnDate { get; private set; }
        [MaxLength(20)]
        public string Contact { get; private set; }
        public int CampusId { get; private set; }
        [ForeignKey("CampusId")]
        public Campus Campus { get; private set; }
        public int IssuanceId { get; private set; }
        [ForeignKey("IssuanceId")]
        public IssuanceMaster Issuance { get; private set; }
        public int StatusId { get; private set; }
        [ForeignKey("StatusId")]
        public WorkFlowStatus Status { get; private set; }
        public virtual List<IssuanceReturnLines> IssuanceReturnLines { get; private set; }

        protected IssuanceReturnMaster()
        {

        }
        public void SetStatus(int statusId)
        {
            StatusId = statusId;
        }

        public void SetIssuanceId(int issuanceeId)
        {
            IssuanceId = issuanceeId;
        }
        public void CreateDocNo()
        {
            //Creating doc no..
            DocNo = "IRTRN-" + String.Format("{0:000}", Id);
        }
    }
}
