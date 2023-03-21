using System.ComponentModel.DataAnnotations;

namespace Application.Contracts.DTOs
{
    public class CourseDto
    {
        public int?Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
