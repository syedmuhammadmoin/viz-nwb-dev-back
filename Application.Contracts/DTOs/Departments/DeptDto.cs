
namespace Application.Contracts.DTOs
{
    public class DeptDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Block { get; set; }
        public string Road { get; set; }
        public string HeadOfDept { get; set; }
        public int OrganizationId { get; set; }
        public string OrgnizationName { get; set; }
    }
}