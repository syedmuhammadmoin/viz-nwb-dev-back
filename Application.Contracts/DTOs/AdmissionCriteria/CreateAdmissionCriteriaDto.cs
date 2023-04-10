using System.ComponentModel.DataAnnotations;

namespace Application.Contracts.DTOs
{
    public class CreateAdmissionCriteriaDto
    {
        public int? Id { get; set; }
        [Required]
        public int? ProgramId { get; set; }
        [Required]
        [MaxLength(200)]
        public string Description { get; set; }
        [Required]
        public int? QualificationId { get; set; }
        [Required]
        public int? SubjectId { get; set; }
        public decimal? QualificationRequriedMarks { get; set; }
        public bool? IsEntryTestRequired { get; set; }
        public DateTime? EntryTestDate { get; set; }
        public decimal? EntryTestRequriedMarks { get; set; }
        public DateTime? InterviewDate { get; set; }
        public bool? IsInterviewRequired { get; set; }
    }
}
