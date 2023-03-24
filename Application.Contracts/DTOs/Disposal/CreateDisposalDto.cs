using System.ComponentModel.DataAnnotations;

namespace Application.Contracts.DTOs
{
    public class CreateDisposalDto
    {
        public int? Id { get; set; }
        [Required]
        public int? FixedAssetId { get; set; }
        public int? BusinessPartnerId { get; set; }
        [Required]
        public DateTime DisposalDate { get; set; }
        [Required]
        [Range(0.00, double.MaxValue, ErrorMessage = "Value must be greater than -1")]
        public decimal DisposalValue { get; set; }
        [Required]
        public bool? IsSubmit { get; set; }
    }
}
