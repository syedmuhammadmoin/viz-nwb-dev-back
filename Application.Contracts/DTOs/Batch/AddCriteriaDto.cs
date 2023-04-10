using System.ComponentModel.DataAnnotations;

namespace Application.Contracts.DTOs
{
    public class AddCriteriaDto
    {
        [Required]
        public int? BatchId { get; set; }
        [Required]
        public int[] CriteriaIds { get; set; }
    }
}
