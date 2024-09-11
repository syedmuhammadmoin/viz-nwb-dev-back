using Domain.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreateTaxDto
    {
        public int? Id { get; set; }
        [Required]
        public string? AccountId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public TaxType TaxType { get; set; }
    }
}
