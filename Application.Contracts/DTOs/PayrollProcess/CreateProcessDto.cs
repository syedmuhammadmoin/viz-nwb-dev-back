using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreateProcessDto
    {
        [Required]
        public int? Month { get; set; }
        [Required]
        public int? Year { get; set; }
        [Required]
        public DateTime? TransDate { get; set; }
        [Required]
        public Guid? AccountPayableId { get; set; }
        [Required]
        public bool? isSubmit { get; set; }
        [Required]
        public List<CreateProcessLinesDto> ProcessLines { get; set; }
    }
}
