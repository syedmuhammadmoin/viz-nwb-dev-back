using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreateProcessLinesDto
    {
        [Required]
        public int WorkingDays { get; set; }
        [Required]
        public int PresentDays { get; set; }
        [Required]
        public int EmployeeId { get; set; }
        [Required]
        [Range(0.00, double.MaxValue, ErrorMessage = "Please enter a positive value")]
        public decimal Tax { get; set; }
    }
}
