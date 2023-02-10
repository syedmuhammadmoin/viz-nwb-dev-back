using Application.Contracts.Filters;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class DepreciationSpecs : BaseSpecification<Depreciation>
    {
        public DepreciationSpecs(TransactionFormFilter filter, bool isTotalRecord)
            : base(c => c.ModelName.Contains(filter.Name != null ? filter.Name : ""))
        {
            if (!isTotalRecord)
            {
                var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
                ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
                AddInclude(i => i.AssetAccount);
                AddInclude(i => i.DepreciationExpense);
                AddInclude(i => i.AccumulatedDepreciation);
                ApplyOrderByDescending(i => i.Id);
            }
        }
        public DepreciationSpecs()
        {
            AddInclude(i => i.AssetAccount);
            AddInclude(i => i.AccumulatedDepreciation);
            AddInclude(i => i.DepreciationExpense);
        }
    }
}
