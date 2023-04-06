using System.ComponentModel.DataAnnotations;

namespace Application.Contracts.DTOs
{
    public class CourseDto
    {
        public int?Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [Range(1, float.MaxValue, ErrorMessage = "Mark must be greater than 0")]
        public decimal? PassingMarks { get; set; }
    }
}
