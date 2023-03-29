using System.ComponentModel.DataAnnotations;

namespace Application.Contracts.DTOs
{
    public class CreateStateDto
    {
        public int? Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
        [Required]
        public int? CountryId { get; set; }
    }
}
