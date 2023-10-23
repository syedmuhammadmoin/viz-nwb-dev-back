using System.ComponentModel.DataAnnotations;

namespace Application.Contracts.DTOs
{
    public class CreateProgramDto
    {
        public int? Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
        [Required]
        public int? DegreeId { get; set; }
        [Required]
        public int? AcademicDepartmentId { get; set; }
        [Required]
        [Range(1, 8, ErrorMessage = "Semester count must be between 1-8")]
        public int? TotalSemesters { get; set; }
        [Required]
        public List<CreateSemesterCourseDto> SemesterCourseList { get; set; }
    }
}
