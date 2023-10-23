namespace Application.Contracts.DTOs
{
    public class ProgramChallanTemplateLinesDto
    {
        public int Id { get; set; }
        public int FeeItemId { get; set; }
        public string FeeItem { get; set; }
        public decimal Amount { get; set; }
        public int MasterId { get; set; }
    }
}
