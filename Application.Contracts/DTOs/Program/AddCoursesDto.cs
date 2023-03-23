using System.ComponentModel.DataAnnotations;

namespace Application.Contracts.DTOs
{
    public class AddCoursesDto
    {
        [Required]
        public int? ProgramId { get; set; }
        [Required]
        public List<AddSemesterCoursesDto> SemesterCousesList { get; set; }
    }
}
