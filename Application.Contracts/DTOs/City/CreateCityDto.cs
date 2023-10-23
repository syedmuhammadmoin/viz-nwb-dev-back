using System.ComponentModel.DataAnnotations;

namespace Application.Contracts.DTOs
{
    public class CreateCityDto
    {
        public int? Id { get; set; }
        [MaxLength(200)]
        [Required]
        public string Name { get; set; }
        [Required]
        public int? StateId { get; set; }
    }
}
