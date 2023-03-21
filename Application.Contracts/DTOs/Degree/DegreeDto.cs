using System.ComponentModel.DataAnnotations;

namespace Application.Contracts.DTOs
{
    public class DegreeDto
    {
        public int? Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
