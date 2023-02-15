using Domain.Constants;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreateFixedAssetDto
    {
        public int? Id { get; set; }
        [Required]
        public DateTime DateofAcquisition { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        public int PurchaseCost { get; set; }
        [Required]
        public int? CategoryId { get; set; }
        [Required]    
        public int SalvageValue { get; set; }
        [Required]
        public int? CampusId { get; set; }
        [Required]
        public int? WarehouseId { get; set; }
        [Required]
        public bool DepreciationApplicability { get; set; }
        public int? DepreciationId { get; set; }
        public DepreciationMethod ModelType { get; set; }
        public Guid? AssetAccountId { get; set; }
        public Guid? DepreciationExpenseId { get; set; }
        public Guid? AccumulatedDepreciationId { get; set; }
        public int? UseFullLife { get; set; }
        [Range(0.00, 100.00, ErrorMessage = "Please enter a value between 0 and 100")]
        public decimal? DecLiningRate { get; set; }
        [Required]
        public bool? isSubmit { get; set; }
        [Required]
        public bool ProrataBasis { get; set; }
        [Required]
        public bool Active { get;  set; }
    }
}
