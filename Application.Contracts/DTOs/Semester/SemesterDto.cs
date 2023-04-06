using Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace Application.Contracts.DTOs
{
    public class SemesterDto
    {
        public int? Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        public Season? Season { get; private set; }
        [Required]
        public DateTime? StartDate { get; private set; }
        [Required]
        public DateTime? EndDate { get; private set; }
        [Required]
        public bool? IsOpenForEnrollment { get; private set; }
        [Required]
        public bool? IsActive { get; private set; }
    }
}
