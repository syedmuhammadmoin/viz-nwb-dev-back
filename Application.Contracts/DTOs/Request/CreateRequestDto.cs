using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreateRequestDto
    {
        public int? Id { get; set; }
        [Required]
        public DateTime? RequestDate { get; set; }
        [Required]
        public int? EmployeeId { get; set; }
        [Required]
        public int? CampusId { get; set; }
        [Required]
        public bool? isSubmit { get; set; }
        public virtual List<CreateRequestLinesDto> RequestLines { get; set; }
    }
}
