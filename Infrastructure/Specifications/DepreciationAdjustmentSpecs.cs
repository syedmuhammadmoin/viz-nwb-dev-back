using Application.Contracts.Filters;
using Domain.Constants;
using Domain.Entities;

namespace Infrastructure.Specifications
{
    public class DepreciationAdjustmentSpecs : BaseSpecification<DepreciationAdjustmentMaster>
    {
        public DepreciationAdjustmentSpecs(TransactionFormFilter filter, bool isTotalRecord)
        {
            if (!isTotalRecord)
            {
                var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
                ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
                AddInclude(i => i.Status);
                AddInclude("DepreciationAdjustmentLines.FixedAsset");
                AddInclude("DepreciationAdjustmentLines.Level4");
                ApplyOrderByDescending(i => i.Id);
            }
        }

        public DepreciationAdjustmentSpecs(bool forEdit)
        {
            if (forEdit)
            {
                AddInclude(i => i.DepreciationAdjustmentLines);
            }
            else
            {
                AddInclude(i => i.Status);
                AddInclude("DepreciationAdjustmentLines.FixedAsset");
                AddInclude("DepreciationAdjustmentLines.Level4");
            }
        }

        public DepreciationAdjustmentSpecs() : base(e => (e.Status.State != DocumentStatus.Unpaid && e.Status.State != DocumentStatus.Partial && e.Status.State != DocumentStatus.Paid && e.Status.State != DocumentStatus.Draft && e.Status.State != DocumentStatus.Cancelled))
        {
            AddInclude(i => i.Status);
            AddInclude("DepreciationAdjustmentLines.FixedAsset.Warehouse");
        }

    }
}
