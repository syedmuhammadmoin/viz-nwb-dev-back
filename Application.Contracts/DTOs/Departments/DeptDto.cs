
namespace Application.Contracts.DTOs
{
    public class DeptDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string HeadOfDept { get; set; }
        public int OrganizationId { get; set; }
        public string OrgnizationName { get; set; }
    }
}