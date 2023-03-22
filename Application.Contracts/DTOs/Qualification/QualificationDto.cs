using System.ComponentModel.DataAnnotations;

namespace Application.Contracts.DTOs
{
    public class QualificationDto
    {
        public int? Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
