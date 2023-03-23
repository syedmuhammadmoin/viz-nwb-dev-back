namespace Application.Contracts.DTOs
{
    public class ProgramDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DegreeId { get; set; }
        public string Degree { get; set; }
        public int AcademicDepartmentId { get; set; }
        public string AcademicDepartment { get; set; }
        public List<SemesterCousesDto> SemesterCoursesList { get; set; }
    }
}
