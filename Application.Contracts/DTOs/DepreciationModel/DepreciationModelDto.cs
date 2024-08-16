using Domain.Constants;

namespace Application.Contracts.DTOs
{
    public class DepreciationModelDto
    {
        public int Id { get; set; }
        public string ModelName { get; set; }
        public int UseFullLife { get; set; }
        public string AssetAccountId { get; set; }
        public string AssetAccount { get; set; }
        public string DepreciationExpenseId { get; set; }
        public string DepreciationExpense { get; set; }
        public string AccumulatedDepreciationId { get; set; }
        public string AccumulatedDepreciation { get; set; }
        public DepreciationMethod ModelType { get; set; }
        public decimal DecliningRate { get; set; }
    }
}
