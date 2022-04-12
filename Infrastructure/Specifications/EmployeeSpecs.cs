using Application.Contracts.Filters;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class EmployeeSpecs : BaseSpecification<Employee>
    {
        public EmployeeSpecs(PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
            ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
            AddInclude(i => i.Department);
            AddInclude(i => i.Designation);
        }

        public EmployeeSpecs()
        {
            AddInclude(i => i.Department);
            AddInclude(i => i.Designation);
        }
    }
}
