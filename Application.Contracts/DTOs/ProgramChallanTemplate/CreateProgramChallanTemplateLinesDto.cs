using System.ComponentModel.DataAnnotations;

namespace Application.Contracts.DTOs
{
    public class CreateProgramChallanTemplateLinesDto
    {
        public int Id { get; set; }
        [Required]
        public int? FeeItemId { get; set; }
        [Required]
        [Range(1, float.MaxValue, ErrorMessage = "Value must be greater than 0")]
        public decimal? Amount { get; set; }
    }
}
