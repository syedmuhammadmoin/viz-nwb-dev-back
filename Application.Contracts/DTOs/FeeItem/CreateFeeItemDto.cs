using System.ComponentModel.DataAnnotations;

namespace Application.Contracts.DTOs
{
    public class CreateFeeItemDto
    {
        public int? Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
        [Required]
        public Guid? AccountId { get; set; }
        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal? Amount { get; set; }
    }
}
