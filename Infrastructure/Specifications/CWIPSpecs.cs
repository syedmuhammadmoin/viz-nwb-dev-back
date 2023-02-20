﻿using Application.Contracts.Filters;
using Domain.Constants;
using Domain.Entities;

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
                AddInclude(i => i.Warehouse);
                AddInclude(i => i.DepreciationModel);
                AddInclude(i => i.DepreciationExpense);
                AddInclude(i => i.AssetAccount);
                AddInclude(i => i.AccumulatedDepreciation);
                AddInclude(i => i.Status);
                ApplyOrderByDescending(i => i.Id);
            }
        }

        public CWIPSpecs()
        {
            AddInclude(i => i.CWIPAccount);
            AddInclude(i => i.Warehouse);
            AddInclude(i => i.DepreciationModel);
            AddInclude(i => i.DepreciationExpense);
            AddInclude(i => i.AssetAccount);
            AddInclude(i => i.AccumulatedDepreciation);
            AddInclude(i => i.Status);
        }

        public CWIPSpecs(string workflow)
        : base(e => (e.Status.State != DocumentStatus.Unpaid && e.Status.State != DocumentStatus.Partial && e.Status.State != DocumentStatus.Paid && e.Status.State != DocumentStatus.Draft && e.Status.State != DocumentStatus.Cancelled))
        {
            AddInclude(i => i.Status);
        }

    }
}
