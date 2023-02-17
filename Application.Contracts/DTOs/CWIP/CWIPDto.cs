using Domain.Constants;

namespace Application.Contracts.DTOs
{
    public class CWIPDto
    {
        public int Id { get; set; }
        public string CwipCode { get; set; }
        public DateTime DateOfAcquisition { get; set; }
        public Guid CWIPAccountId { get; set; }
        public string CWIPAccount { get; set; }
        public int WarehouseId { get; set; }
        public string Warehouse { get; set; }
        public int CostOfAsset { get; set; }
        public int? SalvageValue { get; set; }
        public bool DepreciationApplicability { get; set; }
        public int? DepreciationModelId { get; set; }
        public string DepreciationModel { get; set; }
        public int? UseFullLife { get; set; }
        public Guid? AssetAccountId { get; set; }
        public string AssetAccount { get; set; }
        public Guid? DepreciationExpenseId { get; set; }
        public string DepreciationExpense { get; set; }
        public Guid? AccumulatedDepreciationId { get; set; }
        public string AccumulatedDepreciation { get; set; }
        public DepreciationMethod ModelType { get; set; }
        public decimal DecLiningRate { get; set; }
        public int Quantinty { get; set; }
        public bool ProrataBasis { get; set; }
        public bool IsActive { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public DocumentStatus State { get; set; }
        public IEnumerable<RemarksDto> RemarksList { get; set; }
        public bool IsAllowedRole { get; set; }
    }
}
