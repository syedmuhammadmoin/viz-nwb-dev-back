using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class UpdatePayrollTransactionDto
    {
        [Required]
        public int? Id { get; set; }
        [Required]
        public string? AccountPayableId { get; set; }
        [Required]
        public bool? isSubmit { get; set; }
    }
}
