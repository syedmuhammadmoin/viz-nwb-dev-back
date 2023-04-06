using Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace Application.Contracts.DTOs
{
    public class CreateSemesterCourseDto
    {
        public int Id { get; set; }
        [Required]
        public SemesterNumber? SemesterNumber { get; private set; }
        [Required]
        public int? CourseId { get; private set; }
    }
}
