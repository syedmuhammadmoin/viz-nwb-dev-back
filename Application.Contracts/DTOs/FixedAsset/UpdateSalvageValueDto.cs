using System.ComponentModel.DataAnnotations;

namespace Application.Contracts.DTOs.FixedAsset
{
    public class UpdateSalvageValueDto
    {
        [Required]
        public int? Id { get; set; }
        [Required]
        public int? SalvageValue { get; set; }
        [Required]
        [Range(1.00, int.MaxValue, ErrorMessage = "Value must be greater than 0")]
        public int? UseFullLife { get; set; }
        [Required]
        public bool? IsActive { get; set; }
    }
}
