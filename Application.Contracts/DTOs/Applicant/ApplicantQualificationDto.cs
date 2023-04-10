namespace Application.Contracts.DTOs
{
    public class ApplicantQualificationDto
    {
        public int QualificationId { get; set; }
        public string Qualification { get; set; }
        public int SubjectId { get; set; }
        public string Subject { get; set; }
        public int PassingYear { get; set; }
        public decimal MarksOrGPA { get; set; }
        public string InstituteOrBoard { get; set; }
        public int MasterId { get; set; }
    }
}
