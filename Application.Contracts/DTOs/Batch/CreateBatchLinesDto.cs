using System.ComponentModel.DataAnnotations;

namespace Application.Contracts.DTOs
{
    public class CreateBatchLinesDto
    {
        public int Id { get; set; }
        [Required]
        public int? ProgramId { get; set; }
    }
}
