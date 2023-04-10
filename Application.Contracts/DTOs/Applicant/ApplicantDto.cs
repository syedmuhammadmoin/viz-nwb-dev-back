using Domain.Constants;

namespace Application.Contracts.DTOs
{
    public class ApplicantDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FatherName { get; set; }
        public string CNIC { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
        public DateTime? DateofBirth { get; set; }
        public int? PlaceOfBirthId { get; set; }
        public string PlaceOfBirth { get; set; }
        public string PostalAddress { get; set; }
        public string PostalCode { get; set; }
        public Gender? Gender { get; set; }
        public Religion? Religion { get; set; }
        public int? DomicileId { get; set; }
        public string Domicile { get; set; }
        public int? NationalityId { get; set; }
        public string Nationality { get; set; }
        public MaritalStatus? MaritalStatus { get; set; }
        public virtual List<ApplicantQualificationDto> Qualifications { get; set; }
        public virtual List<ApplicantRelativeDto> Relatives { get; set; }
    }
}
