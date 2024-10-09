using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs.TaxGroup
{
    public class CreateTaxGroupDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int? CountryId { get; set; }    
        public string? Company { get; set; }
        public int? Sequence { get; set; }
        public string? PayableAccountId { get; set; }
        public string? ReceivableAccountId { get; set; }
        public string? AdvanceAccountId { get; set; }     
        public decimal? PreceedingTtl { get; set; }
    }
}
