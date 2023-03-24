using Domain.Constants;

namespace Application.Contracts.DTOs
{
    public class DisposalDto
    {
        public int Id { get; set; }
        public string DocNo { get; set; }

        public int FixedAssetId { get; set; }
        public string FixedAsset { get; set; }
        public int ProductId { get; set; }
        public string Product { get; set; }
        public decimal Cost { get; set; }
        public Guid AssetAcountId { get; set; }
        public string AssetAcount { get; set; }
        public int SalvageValue { get; set; }
        public int UseFullLife { get; set; }
        public Guid AccumulatedDepreciationId { get; set; }
        public string AccumulatedDepreciation { get; set; }
        public decimal AccumulatedDepreciationAmount { get; set; }
        public decimal BookValue { get; set; }
        public DateTime DisposalDate { get; set; }
        public decimal DisposalValue { get; set; }
        public int WarehouseId { get; set; }
        public string Warehouse { get; set; }
        public int? BusinessPartnerId { get; set; }
        public string BusinessPartnerName { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public DocumentStatus State { get; set; }
        public IEnumerable<RemarksDto> RemarksList { get; set; }
        public bool IsAllowedRole { get; set; }
    }   
}
