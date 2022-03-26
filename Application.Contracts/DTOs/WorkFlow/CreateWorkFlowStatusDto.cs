using Domain.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs 
{ 
    public class CreateWorkFlowStatusDto
    {
        public int? Id { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public DocumentStatus State { get; set; }
        [Required]
        public StatusType Type { get; set; }
    }
}
