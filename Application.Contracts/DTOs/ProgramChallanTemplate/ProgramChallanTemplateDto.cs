using Domain.Constants;

namespace Application.Contracts.DTOs
{
    public class ProgramChallanTemplateDto
    {
        public int Id { get; set; }
        public ProgramChallanType ProgramChallanTypeMyProperty { get; set; }
        public int ProgramId { get; set; }
        public string Program { get; set; }
        public int CampusId { get; set; }
        public string Campus { get; set; }
        public int ShiftId { get; set; }
        public string Shift { get; set; }
        public int? SemesterId { get; set; }
        public string Semester { get; set; }
        public int? ExamId { get; set; }
        public Guid BankAccountId { get; set; }
        public string BankAccount { get; set; }
        public string Description { get; set; }
        public decimal LateFeeAfterDueDate { get; set; }
        public DateTime ChallanDate { get; set; }
        public DateTime DueDate { get; set; }
        public List<ProgramChallanTemplateLinesDto> ProgramChallanTemplateLines { get; set; }
    }
}
