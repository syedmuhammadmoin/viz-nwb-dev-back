using Domain.Constants;

namespace Application.Contracts.DTOs
{
    public class ApplicantRelativeDto
    {
        public string FullName { get; set; }
        public Relationship Relationship { get; set; }
        public string CNIC { get; set; }
        public string ContactNo { get; set; }
        public string Occupation { get; set; }
        public int MasterId { get; set; }
    }
}
