using System.ComponentModel.DataAnnotations;

namespace Application.Contracts.DTOs
{
    public class CreateFacultyDto
    {
        public int? Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
    }
}
