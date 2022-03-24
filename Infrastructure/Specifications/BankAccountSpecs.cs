using Application.Contracts.Filters;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class BankAccountSpecs : BaseSpecification<BankAccount>
    {
        public BankAccountSpecs(PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
            ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
        }

        public BankAccountSpecs(Guid clearingAccountId) : base(e => e.ClearingAccountId == clearingAccountId)
        {
        }
    }
}
