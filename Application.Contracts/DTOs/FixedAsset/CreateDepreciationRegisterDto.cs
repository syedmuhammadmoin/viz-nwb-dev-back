namespace Application.Contracts.DTOs
{
    public class CreateDepreciationRegisterDto
    {
        public int FixedAssetId { get; set; }
        public DateTime TransactionDate { get; set; }
        public bool IsAutomatedCalculation { get; set; }
        public decimal DepreciationAmount { get; set; }
        public string Description { get; set; }
        public bool IsGoingtoDispose { get; set; }
        public bool IsSchedule { get; set; }

        


    }
}
