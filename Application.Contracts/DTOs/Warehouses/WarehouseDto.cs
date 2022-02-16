
namespace Application.Contracts.DTOs
{
    public class WarehouseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Block { get; set; }
        public string Road { get; set; }
        public string Manager { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
    }
}