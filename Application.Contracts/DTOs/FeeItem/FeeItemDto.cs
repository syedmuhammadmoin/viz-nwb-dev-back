namespace Application.Contracts.DTOs
{
    public class FeeItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AccountId { get; set; }
        public string Account { get; set; }
        public decimal Amount { get; set; }
    }
}
