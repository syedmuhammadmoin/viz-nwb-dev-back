using Domain.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreatePayrollItemDto
    {
        public int? Id { get; set; }
        [MaxLength(50)]
        [Required]
        public string ItemCode { get; set; }
        [MaxLength(100)]
        [Required]
        public string Name { get; set; }
        [Required]
        public PayrollType PayrollType { get; set; }
        [Required]
        public CalculationType PayrollItemType { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Please enter a positive value")]
        [Required]
        public decimal? Value { get; set; }
        [Required]
        public Guid AccountId { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [MaxLength(300)]
        public string Remarks { get; set; }
        public int[] EmployeeIds { get; set; }
    }
}
