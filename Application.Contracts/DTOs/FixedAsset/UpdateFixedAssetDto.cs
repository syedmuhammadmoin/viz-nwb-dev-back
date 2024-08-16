using Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace Application.Contracts.DTOs
{
    public class UpdateFixedAssetDto
    {
        [Required]
        public int? Id { get; set; }
        [Required]
        public DateTime DateofAcquisition { get; set; }
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
        [Required]
        public decimal Cost { get; set; }
        [Required]
        public int? ProductId { get; set; }
        [Required]
        public int? WarehouseId { get; set; }
        [Required]
        public int SalvageValue { get; set; }
        [Required]
        public bool DepreciationApplicability { get; set; }
        public int? DepreciationModelId { get; set; }
        [Range(1.00, int.MaxValue, ErrorMessage = "Value must be greater than 0")]
        public int? UseFullLife { get; set; }
        public string? AssetAccountId { get; set; }
        public string? DepreciationExpenseId { get; set; }
        public string? AccumulatedDepreciationId { get; set; }
        public DepreciationMethod ModelType { get; set; }
        [Range(0.00, 100.00, ErrorMessage = "Please enter a value between 0 and 100")]
        public decimal? DecLiningRate { get; set; }
        [Required]
        public bool ProrataBasis { get; set; }
        [Required]
        public bool IsActive { get; set; }
        public int? DocId { get; set; }
        public DocType? Doctype { get; set; }
        [Required]
        public bool? IsSubmit { get; set; }
    }
}
