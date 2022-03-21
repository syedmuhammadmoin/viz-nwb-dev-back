using Domain.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreateWorkFlowDto
    {
        public int? Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DocType DocType { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public virtual List<CreateWorkFlowTransitionDto> WorkflowTransitions { get; set; }
    }
}
