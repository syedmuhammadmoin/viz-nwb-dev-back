using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreateCategoryDto
    {
        public int?  Id { get; set; }
        [MaxLength(100)]
        [Required]
        public string Name { get; set; }
        [Required]
        public string? InventoryAccountId { get; set; }
        [Required]
        public string? RevenueAccountId { get; set; }
        [Required]
        public string? CostAccountId { get; set; }
        [Required]
        public bool? IsFixedAsset { get; set; }
        public int? DepreciationModelId { get; set; }
    }
}
