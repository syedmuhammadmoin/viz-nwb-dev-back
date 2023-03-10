using Domain.Constants;

namespace Application.Contracts.DTOs
{
    public class DepreciationAdjustmentDto
    {
        public int Id { get; set; }
        public string DocNo { get; set; }
        public DateTime DateOfDepreciationAdjustment { get;  set; }
        public string Description { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public DocumentStatus State { get; set; }
        public int? TransactionId { get; set; }

        public bool IsAllowedRole { get; set; }
        public IEnumerable<RemarksDto> RemarksList { get; set; }
        public virtual List<DepreciationAdjustmentLinesDto> DepreciationAdjustmentLines { get;  set; }
    }
}
