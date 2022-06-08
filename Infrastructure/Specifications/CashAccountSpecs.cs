using Application.Contracts.Filters;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class CashAccountSpecs : BaseSpecification<CashAccount>
    {
        public CashAccountSpecs(TransactionFormFilter filter) 
            : base(c => (c.CashAccountName.Contains(filter.Name != null 
                ? filter.Name : "")))
        {
            var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
            ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
            AddInclude(i => i.Campus);
            ApplyOrderByDescending(i => i.Id);
        }
    }
}
