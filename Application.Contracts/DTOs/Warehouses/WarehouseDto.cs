
namespace Application.Contracts.DTOs
{
    public class WarehouseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Manager { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
    }
}