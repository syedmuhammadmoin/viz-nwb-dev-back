
namespace Application.Contracts.DTOs
{
    public class WarehouseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StoreManager { get; set; }
        public string CampusName { get; set; }
        public int CampusId{ get; set; }
    }
}