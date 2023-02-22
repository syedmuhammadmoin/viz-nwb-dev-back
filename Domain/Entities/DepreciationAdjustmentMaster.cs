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
    public class DepreciationAdjustmentMaster : BaseEntity<int>
    {
        public DateTime DateOfDepreciationAdjustment { get; private set; }
        public int CampusId { get; private set; }
        [ForeignKey("CampusId")]
        public Campus Campus { get; private set; }
        [MaxLength(500)]
        public string Description { get; private set; }
        public int StatusId { get; private set; }
        [ForeignKey("StatusId")]
        public WorkFlowStatus Status { get; private set; }
        public virtual List<DepreciationAdjustmentLines> DepreciationAdjustmentLines { get; private set; }

        protected DepreciationAdjustmentMaster()
        {
        }

        public void SetStatus(int statusId)
        {
            StatusId = statusId;
        }
    }
}
