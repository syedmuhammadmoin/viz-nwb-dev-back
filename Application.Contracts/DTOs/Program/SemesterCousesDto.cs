namespace Application.Contracts.DTOs
{
    public class SemesterCousesDto
    {
        public int SemesterId { get; set; }
        public string Semester { get; set; }
        public int CourseId { get; set; }
        public string Course { get; set; }
    }
}
