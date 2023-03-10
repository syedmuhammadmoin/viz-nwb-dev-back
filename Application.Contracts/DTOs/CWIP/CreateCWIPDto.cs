using Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace Application.Contracts.DTOs
{
    public class CreateCWIPDto
    {
        public int? Id { get; set; }
        [Required]
        public DateTime DateOfAcquisition { get; set; }
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
        [Required]
        public Guid CWIPAccountId { get; set; }
        [Required]
        public int? Cost { get; set; }
        [Required]
        public int? ProductId { get; set; }
        [Required]
        public int? WarehouseId { get; set; }
        public int? SalvageValue { get; set; }
        [Required]
        public bool DepreciationApplicability { get; set; }
        public int? DepreciationModelId { get; set; }
        public int? UseFullLife { get; set; }
        public Guid? AssetAccountId { get; set; }
        public Guid? DepreciationExpenseId { get; set; }
        public Guid? AccumulatedDepreciationId { get; set; }
        public DepreciationMethod ModelType { get; set; }
        [Range(0.00, 100.00, ErrorMessage = "Please enter a value between 0 and 100")]
        public decimal? DecLiningRate { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public bool ProrataBasis { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public bool? IsSubmit { get; set; }

    }
}
