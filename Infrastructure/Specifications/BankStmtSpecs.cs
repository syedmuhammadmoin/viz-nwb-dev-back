using Application.Contracts.Filters;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class BankStmtSpecs : BaseSpecification<BankStmtMaster>
    {
        public BankStmtSpecs(PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
            ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
            ApplyOrderByDescending(i => i.Id);
            AddInclude(i => i.BankAccount);
            AddInclude(i => i.BankStmtLines);
        }
        public BankStmtSpecs()
        {
            AddInclude(i => i.BankAccount);
            AddInclude(i => i.BankStmtLines);
        }
    }
}
