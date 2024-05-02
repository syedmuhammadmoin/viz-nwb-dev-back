using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreatePayrollTransactionDto
    {
        [Required]
        [Range(1, 12, ErrorMessage = "Please enter months from 1 - 12")] 
        public int? Month { get; set; }
        [Required]
        public int? Year { get; set; }
        [Required]
        [StringLength(13, MinimumLength = 13, ErrorMessage ="Please enter valid CNIC Number of 13 Digits without Dashes")] 
        public string EmployeeCNIC { get; set; }
        [Required]
        [Range(1, 31, ErrorMessage = "Please enter working days between 1 - 31")]
        public int? WorkingDays { get; set; }
        [Required]
        [Range(0, 31, ErrorMessage = "Please enter present days between 0 - 31")]
        public int? PresentDays { get; set; }
        [Required]
        [Range(0, 31, ErrorMessage = "Please enter leave days between 0 - 31")]
        public int? LeaveDays { get; set; }
        [Required]
        public DateTime TransDate { get; set; }
        

    }
}
