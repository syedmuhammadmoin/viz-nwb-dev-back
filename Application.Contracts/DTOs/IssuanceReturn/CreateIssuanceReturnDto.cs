using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{ 
    public class CreateIssuanceReturnDto
    {
        public int? Id { get; set; }
        [Required]
        public int? EmployeeId { get; set; }
        [Required]
        public DateTime? IssuanceReturnDate { get; set; }
        [MaxLength(20)]
        public string Contact { get; set; }
        [Required]
        public int? CampusId { get; set; }
        [Required]
        public int? IssuanceId { get; set; }
        [Required]
        public bool? isSubmit { get; set; }
        public virtual List<CreateIssuanceReturnLinesDto> IssuanceReturnLines { get; set; }
    }
}
