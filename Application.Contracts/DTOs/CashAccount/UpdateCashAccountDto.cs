using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class UpdateCashAccountDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string CashAccountName { get; set; }
        [Required]
        [MaxLength(10)]
        public string AccountCode { get; set; }
        [StringLength(200)]
        public string Handler { get; set; }
    }
}
