namespace Application.Contracts.DTOs
{
    public class CityDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int StateId { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }
}
