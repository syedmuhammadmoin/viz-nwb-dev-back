using Application.Contracts.Filters;
using Domain.Constants;
using Domain.Entities;

namespace Infrastructure.Specifications
{
    public class FixedAssetSpecs : BaseSpecification<FixedAsset>
    {
        public FixedAssetSpecs(TransactionFormFilter filter, bool isTotalRecord)
           : base(c => c.Product.ProductName.Contains(filter.Name != null ? filter.Name : ""))
        {
            if (!isTotalRecord)
            {
                var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
                ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
                AddInclude(i => i.Product);
                AddInclude(i => i.Warehouse);
                AddInclude(i => i.DepreciationModel);
                AddInclude(i => i.AssetAccount);
                AddInclude(i => i.DepreciationExpense);
                AddInclude(i => i.AccumulatedDepreciation);
                AddInclude(i => i.Status);
                ApplyOrderByDescending(i => i.Id);
            }
        }

        public FixedAssetSpecs()
        {
            AddInclude(i => i.Product);
            AddInclude(i => i.Warehouse);
            AddInclude(i => i.DepreciationModel);
            AddInclude(i => i.AssetAccount);
            AddInclude(i => i.DepreciationExpense);
            AddInclude(i => i.AccumulatedDepreciation);
            AddInclude(i => i.Status);
        }

        public FixedAssetSpecs(string workflow)
        : base(e => (e.Status.State != DocumentStatus.Unpaid && e.Status.State != DocumentStatus.Partial && e.Status.State != DocumentStatus.Paid && e.Status.State != DocumentStatus.Draft && e.Status.State != DocumentStatus.Cancelled))
        {
            AddInclude(i => i.Status);
        }

        public FixedAssetSpecs(bool IsDisposable)
            : base(e => (e.IsHeldforSaleOrDisposal == true))
        {
        }

        public FixedAssetSpecs(int ProductId)
            : base(e => (
            e.ProductId == ProductId
            && e.IsHeldforSaleOrDisposal == false
            && e.IsIssued== false
            && e.Status.State == DocumentStatus.Unpaid))
            
        {
            AddInclude(i => i.Status);
        }

        public FixedAssetSpecs(bool isDisposed, bool isHeldforSaleOrDisposal,
            bool depreciationApplicability) : base(i => i.IsDisposed == isDisposed && i.IsHeldforSaleOrDisposal == isHeldforSaleOrDisposal && i.DepreciationApplicability == depreciationApplicability)
        {

        }
    }
}
