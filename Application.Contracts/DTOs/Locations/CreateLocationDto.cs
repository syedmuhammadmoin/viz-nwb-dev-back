using System.ComponentModel.DataAnnotations;

namespace Application.Contracts.DTOs
{
    public class CreateLocationDto
    {
        public int? Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string Dimensions { get; set; }
        [MaxLength(50)]
        public string Supervisor { get; set; }
        [Required]
        public int WarehouseId { get; set; }
    }
}