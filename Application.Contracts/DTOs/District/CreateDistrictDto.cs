using System.ComponentModel.DataAnnotations;

namespace Application.Contracts.DTOs
{
    public class CreateDistrictDto
    {
        public int? Id { get; set; }
        [MaxLength(200)]
        [Required]
        public string Name { get; set; }
        [Required]
        public int? CityId { get; set; }
    }
}
