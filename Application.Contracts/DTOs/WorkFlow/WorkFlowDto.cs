using Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class WorkFlowDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DocType DocType { get; set; }
        public bool IsActive { get; set; }
        public virtual List<WorkFlowTransitionDto> WorkflowTransitions { get; set; }
    }
}
