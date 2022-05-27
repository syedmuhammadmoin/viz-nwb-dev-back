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
        public int? Id { get; set; }
        [Required]
        public int Month { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        public int EmployeeId { get; set; }
        [Required]
        public int WorkingDays { get; set; }
        [Required]
        public int PresentDays { get; set; }
        [Required]
        public int LeaveDays { get; set; }
        [Required]
        public DateTime TransDate { get; set; }
        [Required]
        public Guid AccountPayableId { get; set; }
        [Required]
        public bool isSubmit { get; set; }
    }
}
