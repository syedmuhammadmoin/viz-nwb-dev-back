using System.ComponentModel.DataAnnotations;

namespace Application.Contracts.DTOs
{
    public class CreateFeeItemDto
    {
        public int? Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
        [Required]
        public Guid? AccountId { get; set; }
    }
}
