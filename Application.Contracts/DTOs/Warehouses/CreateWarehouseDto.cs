using System.ComponentModel.DataAnnotations;

namespace Application.Contracts.DTOs
{
    public class CreateWarehouseDto
    {
        public int? Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(100)]
        public string StoreManager { get; set; }
        [Required]
        public int? CampusId { get; set; }
    }
}