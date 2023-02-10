using Application.Contracts.Filters;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class CategorySpecs : BaseSpecification<Category>
    {
        public CategorySpecs(TransactionFormFilter filter, bool isTotalRecord)
            : base(c => c.Name.Contains(filter.Name != null ? filter.Name : ""))
        {
            if (!isTotalRecord)
            {
                var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
                ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
                ApplyOrderByDescending(i => i.Id);
                AddInclude(i => i.InventoryAccount);
                AddInclude(i => i.CostAccount);
                AddInclude(i => i.RevenueAccount);
                AddInclude(i => i.Depreciation);
            } 
        }
        public CategorySpecs()
        {
            AddInclude(i => i.InventoryAccount);
            AddInclude(i => i.CostAccount);
            AddInclude(i => i.RevenueAccount);
            AddInclude(i => i.Depreciation);
        }
        public CategorySpecs(int Id) : base(c => c.IsFixedAsset == true)
        {
            AddInclude(i => i.InventoryAccount);
            AddInclude(i => i.CostAccount);
            AddInclude(i => i.RevenueAccount);
            AddInclude(i => i.Depreciation);
        }
    }
}
