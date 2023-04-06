using Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Application.Contracts.DTOs
{
    public class CreateBatchDto
    {
        public int? Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public int? SemesterId { get; set; }
        [Required]
        public int? CampusId { get; set; }
        [Required]
        public int? ShiftId { get; set; }
        [Required]
        public bool? IsAdmissionOpen { get; set; }
        public virtual List<CreateBatchLinesDto> BatchLines { get; set; }
    }
}
