using Domain.Base;
using Domain.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class WorkFlowMaster : BaseEntity<int>
    {
        [MaxLength(100)]
        public string Name { get; private set; }
        public DocType DocType { get; private set; }
        public bool IsActive { get; private set; }
        public virtual List<WorkFlowTransition> WorkflowTransitions { get; private set; }

        protected WorkFlowMaster()
        {

        }
    }
}
