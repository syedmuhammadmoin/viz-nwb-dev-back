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
    public class CWIPSpecs : BaseSpecification<CWIP>
    {
        public CWIPSpecs(TransactionFormFilter filter, bool isTotalRecord)
        {
            if (!isTotalRecord)
            {
                var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
                ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
                AddInclude(i => i.CWIPAccount);
                AddInclude(i => i.AssetAccount);
                AddInclude(i => i.DepreciationExpense);
                AddInclude(i => i.AccumulatedDepreciation);
                AddInclude(i => i.Depreciation);
                AddInclude(i => i.Status);
                AddInclude(i => i.Campus);
                AddInclude(i => i.Warehouse);
                ApplyOrderByDescending(i => i.Id);
            }
        }

        public CWIPSpecs()
        {
            AddInclude(i => i.CWIPAccount);
            AddInclude(i => i.AssetAccount);
            AddInclude(i => i.DepreciationExpense);
            AddInclude(i => i.Campus);
            AddInclude(i => i.AccumulatedDepreciation);
            AddInclude(i => i.Status);
            AddInclude(i => i.Depreciation);
            AddInclude(i => i.Warehouse);
        }
        public CWIPSpecs(string workflow)
        : base(e => (e.Status.State != DocumentStatus.Unpaid && e.Status.State != DocumentStatus.Partial && e.Status.State != DocumentStatus.Paid && e.Status.State != DocumentStatus.Draft && e.Status.State != DocumentStatus.Cancelled))
        {
            AddInclude(i => i.Status);
        }
    }

}
