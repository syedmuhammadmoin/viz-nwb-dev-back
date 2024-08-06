using Domain.Base;
using Domain.Constants;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class WorkFlowTransition : BaseEntity<int>
    {
        public int CurrentStatusId { get; private set; }
        [ForeignKey("CurrentStatusId")]
        public WorkFlowStatus CurrentStatus { get; private set; }
        public ActionButton Action { get; private set; }
        public int NextStatusId { get; private set; }
        [ForeignKey("NextStatusId")]
        public WorkFlowStatus NextStatus { get; private set; }
        public string AllowedRoleId { get; private set; }
        [ForeignKey("AllowedRoleId")]
        public virtual Role AllowedRole { get; private set; }
        public int MasterId { get; private set; }
        [ForeignKey("MasterId")]
        public WorkFlowMaster WorkflowMaster { get; private set; }

        protected WorkFlowTransition()
        {

        }
    }
}
