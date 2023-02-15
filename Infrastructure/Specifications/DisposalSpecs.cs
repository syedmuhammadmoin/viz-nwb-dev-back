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
    public class DisposalSpecs : BaseSpecification<Disposal>
    {
        public DisposalSpecs(TransactionFormFilter filter, bool isTotalRecord)
             : base(c => c.Category.Name.Contains(filter.Name != null ? filter.Name : ""))
        {
            if (!isTotalRecord)
            {
                var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
                ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
                AddInclude(i => i.Asset.AssetAccount);
                AddInclude(i => i.AccumulatedDepreciation);
                AddInclude(i => i.Category);
                AddInclude(i => i.Warehouse);
                AddInclude(i => i.Status);
                ApplyOrderByDescending(i => i.Id);
            }
        }
        public DisposalSpecs()
        {
            AddInclude(i => i.Asset.AssetAccount);
            AddInclude(i => i.AccumulatedDepreciation);
            AddInclude(i => i.Category);
            AddInclude(i => i.Warehouse);
            AddInclude(i => i.Status);
        }
        public DisposalSpecs(string workflow)
       : base(e => (e.Status.State != DocumentStatus.Unpaid && e.Status.State != DocumentStatus.Partial && e.Status.State != DocumentStatus.Paid && e.Status.State != DocumentStatus.Draft && e.Status.State != DocumentStatus.Cancelled))
        {
            AddInclude(i => i.Status);
        }
    }
}
