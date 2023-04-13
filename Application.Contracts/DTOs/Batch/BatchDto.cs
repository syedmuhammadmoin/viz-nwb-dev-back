namespace Application.Contracts.DTOs
{
    public class BatchDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public int SemesterId { get; set; }
        public string Semester { get; set; }
        public int CampusId { get; set; }
        public string Campus { get; set; }
        public int ShiftId { get; set; }
        public string Shift { get; set; }
        public bool IsAdmissionOpen { get; set; }
        public virtual List<BatchLinesDto> BatchLines { get; set; }
        public virtual List<AdmissionCriteriaDto> AdmissionCriteria { get; set; }
    }
}
