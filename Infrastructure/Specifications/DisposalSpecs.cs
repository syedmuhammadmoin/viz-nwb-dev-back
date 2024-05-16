﻿using Application.Contracts.Filters;
using Domain.Constants;
using Domain.Entities;

namespace Infrastructure.Specifications
{
    public class DisposalSpecs : BaseSpecification<Disposal>
    {
        public DisposalSpecs(int transactionId) :
            base(p => (p.Status.State == DocumentStatus.Unpaid
             || p.Status.State == DocumentStatus.Partial) && (p.TransactionId == transactionId))
        {

            AddInclude(i => i.Status);
        }
        public DisposalSpecs(TransactionFormFilter filter, bool isTotalRecord)
             : base(c => c.Product.ProductName.Contains(filter.Name != null ? filter.Name : "")
             &&
			 c.DisposalDate.Month == (filter.Month != null ? Convert.ToInt32(filter.Month) : c.DisposalDate.Month)
			&& c.DisposalDate.Year == (filter.Year != null ? Convert.ToInt32(filter.Year) : c.DisposalDate.Year))
        {
            if (!isTotalRecord)
            {
                var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
                ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
                AddInclude(i => i.FixedAsset);
                AddInclude(i => i.Product);
                AddInclude(i => i.AccumulatedDepreciation);
                AddInclude(i => i.Warehouse);
                AddInclude(i => i.Status);
                ApplyOrderByDescending(i => i.Id);
            }
        }
        
        public DisposalSpecs()
        {
            AddInclude(i => i.FixedAsset);
            AddInclude(i => i.Product);
            AddInclude(i => i.AccumulatedDepreciation);
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
