using Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace Application.Contracts.DTOs
{
    public class CreateDepreciationModelDto
    {
        public int? Id { get; set; }
        [Required]
        public string ModelName { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Use full life must be greater than 0")]
        public int? UseFullLife { get; set; }
        [Required]
        public Guid AssetAccountId { get; set; }
        [Required]
        public Guid DepreciationExpenseId { get; set; }
        [Required]
        public Guid AccumulatedDepreciationId { get; set; }
        [Required]
        public DepreciationMethod ModelType { get; set; }
        [Range(0.00, 100.00, ErrorMessage = "Please enter a value between 0 and 100")]
        public decimal DecliningRate { get; set; }
    }
}
