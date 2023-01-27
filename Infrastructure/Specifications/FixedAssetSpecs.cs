using Application.Contracts.Filters;
using Domain.Constants;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class FixedAssetSpecs : BaseSpecification<FixedAsset>
    {
        public FixedAssetSpecs(TransactionFormFilter filter, bool isTotalRecord)
           : base(c => c.Category.Name.Contains(filter.Name != null ? filter.Name : "") )
        {
            if (!isTotalRecord)
            {
                var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
                ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
                AddInclude(i => i.Category);
                AddInclude(i => i.AssetAccount);
                AddInclude(i => i.DepreciationExpense);
                AddInclude(i => i.AccumulatedDepreciation);
                AddInclude(i => i.Depreciation);
                ApplyOrderByDescending(i => i.Id);
            }
        }
        public FixedAssetSpecs()
        {
            AddInclude(i => i.Category);
            AddInclude(i => i.AssetAccount);
            AddInclude(i => i.DepreciationExpense);
            AddInclude(i => i.AccumulatedDepreciation);
            AddInclude(i => i.Depreciation);
        }
    }
}
