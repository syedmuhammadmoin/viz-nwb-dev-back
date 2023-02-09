using Domain.Constants;
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
    public class CreateCWIPDto
    {
        public int? Id { get; set; }
        [Required]
        public DateTime DateOfAcquisition { get; set; }
        [Required]
        public Guid CWIPAccountId { get; set; }
        [Required]
        public int? CostOfAsset { get; set; }
        [Required]
        public Guid AssetAccountId { get; set; }
        public int? SalvageValue { get; set; }
        [Required]
        public int? CampusId { get; set; }
        [Required]
        public int? WareHouseId { get; set; }
        [Required]
        public bool DepreciationApplicability { get; set; }
        public int? DepreciationId { get; set; }
        public DepreciationMethod ModelType { get; set; }
        public Guid? DepreciationExpenseId { get; set; }
        public Guid? AccumulatedDepreciationId { get; set; }
        public int? UseFullLife { get; set; }
        public int Quantinty { get; set; }
        [Range(0.00, 100.00, ErrorMessage = "Please enter a value between 0 and 100")]
        public decimal? DecLiningRate { get; set; }
        [Required]
        public bool ProrataBasis { get; set; }
        [Required]
        public bool Active { get; set; }
        [Required]
        public bool? isSubmit { get; set; }

    }
}
