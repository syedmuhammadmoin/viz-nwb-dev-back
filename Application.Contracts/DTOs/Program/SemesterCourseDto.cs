using Domain.Constants;

namespace Application.Contracts.DTOs
{
    public class SemesterCourseDto
    {
        public int Id { get; set; }
        public SemesterNumber SemesterNumber { get; set; }
        public int CourseId { get; set; }
        public string Course { get; set; }
    }
}
