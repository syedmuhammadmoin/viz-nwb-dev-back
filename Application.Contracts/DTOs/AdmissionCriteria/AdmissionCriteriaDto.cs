namespace Application.Contracts.DTOs
{
    public class AdmissionCriteriaDto
    {
        public int Id { get; set; }
        public int ProgramId { get; set; }
        public string Program { get; set; }
        public string Description { get; set; }
        public int QualificationId { get; set; }
        public string Qualification { get; set; }
        public int SubjectId { get; set; }
        public string Subject { get; set; }
        public decimal? QualificationRequriedMarks { get; set; }
        public bool? IsEntryTestRequired { get; set; }
        public DateTime? EntryTestDate { get; set; }
        public decimal? EntryTestRequriedMarks { get; set; }
        public DateTime? InterviewDate { get; set; }
        public bool? IsInterviewRequired { get; set; }
    }
}
