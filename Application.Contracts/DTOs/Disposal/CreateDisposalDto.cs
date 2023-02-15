using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Constants;

namespace Application.Contracts.DTOs
{
    public class CreateDisposalDto
    {
        public int? Id { get; set; }
        [Required]
        public int? AssetId { get; set; }
        [Required]
        public int? CategoryId { get; set; }
        [Required]
        public decimal PurchaseCost { get; set; }
        [Required]
        public int SalvageValue { get; set; }
        [Required]
        public int? UsefulLife { get; set; }
        [Required]
        public Guid? AccumulatedDepreciationId { get; set; }
        [Required]
        public decimal BookValue { get; set; }
        [Required]
        public DateTime DisposalDate { get; set; }
        [Required]
        public decimal DisposalValue { get; set; }
        [Required]   
        public int? WarehouseId { get; set; }
        [Required]
        public bool? isSubmit { get; set; }
    }
}
