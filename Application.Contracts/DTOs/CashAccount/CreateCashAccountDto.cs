using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreateCashAccountDto
    {
        public int? Id { get; set; }
        [Required]
        public string CashAccountName { get; set; }
        public string Handler { get; set; }
        [Required]
        [Range(0.00, double.MaxValue, ErrorMessage = "Please enter a positive value")]
        public decimal OpeningBalance { get; set; }
        public string Currency { get; set; }
    }
}
