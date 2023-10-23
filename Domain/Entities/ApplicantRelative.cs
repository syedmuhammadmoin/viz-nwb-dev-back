using Domain.Base;
using Domain.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class ApplicantRelative : BaseEntity<int>
    {
        [MaxLength(200)]
        public string FullName { get; private set; }
        public Relationship Relationship { get; private set; }
        [MaxLength(20)]
        public string CNIC { get; private set; }
        [MaxLength(15)]
        public string ContactNo { get; private set; }
        [MaxLength(200)]
        public string Occupation { get; set; }

        public int MasterId { get; private set; }
        [ForeignKey("MasterId")]
        public Applicant Applicant { get; private set; }

        protected ApplicantRelative()
        {
        }
    }
}
