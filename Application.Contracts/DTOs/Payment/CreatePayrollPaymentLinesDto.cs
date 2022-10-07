using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreatePayrollPaymentLinesDto
    {
        public int LedgerId { get; set; }
        [Required]
        public int? BusinessPartnerId { get; set; }
        [Required]
        public int? CampusId { get; set; }
        public Guid AccountPayableId { get; set; }
        [Range(1, double.MaxValue, ErrorMessage = "Gross amount must be greater than 0")]
        public decimal NetSalary { get; set; }

    }
}
