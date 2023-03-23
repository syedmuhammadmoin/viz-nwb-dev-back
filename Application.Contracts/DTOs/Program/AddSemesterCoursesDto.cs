using System.ComponentModel.DataAnnotations;

namespace Application.Contracts.DTOs
{
    public class AddSemesterCoursesDto
    {
        [Required]
        public int? SemesterId { get; set; }
        [Required]
        public int? CourseId { get; set; }
    }
}
