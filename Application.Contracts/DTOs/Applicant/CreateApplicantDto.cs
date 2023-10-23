using Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace Application.Contracts.DTOs
{
    public class CreateApplicantDto
    {
        [Required]
        public DateTime? DateofBirth { get; set; }
        [Required]
        public int? PlaceOfBirthId { get; set; }
        [Required]
        [MaxLength(200)]
        public string PostalAddress { get; set; }
        [Required]
        [MaxLength(10)]
        public string PostalCode { get; set; }
        [Required]
        public Gender? Gender { get; set; }
        [Required]
        public Religion? Religion { get; set; }
        [Required]
        public int? DomicileId { get; set; }
        [Required]
        public int? NationalityId { get; set; }
        [Required]
        public MaritalStatus? MaritalStatus { get; set; }
        [Required]
        public virtual List<CreateApplicantQualificationDto> Qualifications { get; set; }
        [Required]
        public virtual List<CreateApplicantRelativeDto> Relatives { get; set; }
    }
}
