using Domain.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreateWorkFlowTransitionDto
    {
        public int? Id { get; set; }
        [Required]
        public int? CurrentStatusId { get; set; }
        [Required]
        public ActionButton? Action { get; set; }
        [Required]
        public int? NextStatusId { get; set; }
        [Required]
        public string AllowedRoleId { get; set; }
    }
}
