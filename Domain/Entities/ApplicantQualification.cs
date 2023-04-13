using Domain.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class ApplicantQualification : BaseEntity<int>
    {
        public int QualificationId { get; private set; }
        [ForeignKey("QualificationId")]
        public Qualification Qualification { get; private set; }

        public int SubjectId { get; private set; }
        [ForeignKey("SubjectId")]
        public Subject Subject { get; private set; }

        public int PassingYear { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal MarksOrGPA { get; private set; }
        [MaxLength(200)]
        public string InstituteOrBoard { get; private set; }

        public int MasterId { get; private set; }
        [ForeignKey("MasterId")]
        public Applicant Applicant { get; private set; }

        protected ApplicantQualification()
        {
        }
    }
}
