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
        [MaxLength(50)]
        public string Country { get; set; }
        [Required]
        [MaxLength(50)]
        public string State { get; set; }
        [Required]
        [MaxLength(50)]
        public string City { get; set; }
        [MaxLength(50)]
        public string Street { get; set; }
        [MaxLength(50)]
        public string Block { get; set; }
        [MaxLength(50)]
        public string Road { get; set; }
        [MaxLength(100)]
        public string Manager { get; set; }
        [Required]
        public int DepartmentId { get; set; }
    }
}