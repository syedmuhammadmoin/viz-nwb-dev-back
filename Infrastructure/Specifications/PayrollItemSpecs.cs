using Application.Contracts.Filters;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class PayrollItemSpecs : BaseSpecification<PayrollItem>
    {
        public PayrollItemSpecs(PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
            ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
            AddInclude(i => i.Account);
        }
        public PayrollItemSpecs(int payrollItemId) : base(e => e.Id == payrollItemId)
        {

        }

        public PayrollItemSpecs() 
        {
            AddInclude(i => i.Account);
        }
    }
}
