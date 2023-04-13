using System.ComponentModel.DataAnnotations;

namespace Application.Contracts.DTOs
{
    public class CreateAdmissionApplicationDto
    {
        public int? Id { get; set; }
        public CreateApplicantDto ApplicantDetails { get; set; }
        public int? StudentId { get; set; }
        [Required]
        public int? BatchId { get; set; }
        [Required]
        public int? ProgramId { get; set; }
        [Required]
        public int? CampusId { get; set; }
        [Required]
        public int? ShiftId { get; set; }
    }
}
