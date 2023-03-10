namespace Application.Contracts.DTOs
{
    public class AcademicDepartmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int FacultyId { get; set; }
        public string Faculty { get; set; }
    }
}
