using Domain.Base;
using Domain.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Applicant : BaseEntity<int>
    {
        [MaxLength(100)]
        public string Name { get; private set; }
        [MaxLength(100)]
        public string FatherName { get; private set; }
        [MaxLength(20)]
        public string CNIC { get; private set; }
        [MaxLength(15)]
        public string ContactNo { get; private set; }
        [MaxLength(100)]
        public string Email { get; private set; }
        public DateTime? DateofBirth { get; private set; }

        public int? PlaceOfBirthId { get; private set; }
        [ForeignKey("PlaceOfBirthId")]
        public City PlaceOfBirth { get; private set; }

        [MaxLength(200)]
        public string PostalAddress { get; private set; }
        [MaxLength(10)]
        public string PostalCode { get; private set; }
        public Gender? Gender { get; private set; }
        public Religion? Religion { get; private set; }

        public int? DomicileId { get; private set; }
        [ForeignKey("DomicileId")]
        public Domicile Domicile { get; private set; }

        public int? NationalityId { get; private set; }
        [ForeignKey("NationalityId")]
        public Country Nationality { get; private set; }

        public MaritalStatus? MaritalStatus { get; private set; }

        public int BusinessPartnerId { get; private set; }
        [ForeignKey("BusinessPartnerId")]
        public BusinessPartner BusinessPartner { get; private set; }

        public string UserId { get; private set; }
        [ForeignKey("UserId")]
        public User User { get; private set; }

        public virtual List<ApplicantQualification> Qualifications { get; private set; }
        public virtual List<ApplicantRelative> Relatives { get; private set; }

        protected Applicant()
        {
        }

        public void SetBusinessPartnerId(int id)
        {
            BusinessPartnerId = id;
        }

        public void SetUserId(string id)
        {
            UserId = id;
        }

    }
}
