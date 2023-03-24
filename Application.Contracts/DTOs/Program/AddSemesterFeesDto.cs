using System.ComponentModel.DataAnnotations;

namespace Application.Contracts.DTOs
{
    public class AddSemesterFeesDto
    {
        [Required]
        public int? ProgramSemesterId { get; set; }
        [Required]
        public int? FeeItemId { get; set; }
        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal? Amount { get; set; }
    }
}
