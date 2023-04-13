using System.ComponentModel.DataAnnotations;

namespace Application.Contracts.DTOs
{
    public class CreateApplicantQualificationDto
    {
        public int Id { get; set; }
        [Required]
        public int? QualificationId { get; set; }
        [Required]
        public int? SubjectId { get; set; }
        [Required]
        public int? PassingYear { get; set; }
        [Required]
        public decimal MarksOrGPA { get; set; }
        [Required]
        [MaxLength(200)]
        public string InstituteOrBoard { get; set; }
    }
}
