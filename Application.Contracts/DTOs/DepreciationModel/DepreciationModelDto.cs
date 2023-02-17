using Domain.Constants;

namespace Application.Contracts.DTOs
{
    public class DepreciationModelDto
    {
        public int Id { get; set; }
        public string ModelName { get; set; }
        public int UseFullLife { get; set; }
        public Guid AssetAccountId { get; set; }
        public string AssetAccount { get; set; }
        public Guid DepreciationExpenseId { get; set; }
        public string DepreciationExpense { get; set; }
        public Guid AccumulatedDepreciationId { get; set; }
        public string AccumulatedDepreciation { get; set; }
        public DepreciationMethod ModelType { get; set; }
        public decimal DecliningRate { get; set; }
    }
}
