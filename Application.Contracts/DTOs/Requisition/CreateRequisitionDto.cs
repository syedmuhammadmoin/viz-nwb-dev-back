using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreateRequisitionDto
    {
        public int? Id { get; set; }
        [Required]
        public int? EmployeeId { get; set; }
        [Required]
        public DateTime? RequisitionDate { get; set; }
        [Required]
        public int? CampusId { get; set; }
        [Required]
        public bool? IsWithoutWorkflow { get; set; }
        [Required]
        public bool? isSubmit { get; set; }
        public virtual List<CreateRequisitionLinesDto> RequisitionLines { get; set; }
    }
}
