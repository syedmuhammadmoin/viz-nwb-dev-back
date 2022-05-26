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
            ApplyOrderByDescending(i => i.Id);
            AddInclude(i => i.Department);
            AddInclude(i => i.Designation);
        }

        public EmployeeSpecs()
        {
            AddInclude(i => i.Department);
            AddInclude(i => i.Designation);
        }

        public EmployeeSpecs(string getCNIC) : base(e => e.CNIC == getCNIC)
        {

        }

        public EmployeeSpecs(bool isActive, int?[] departmentIds) 
            : base(c => c.isActive == isActive &&
                (departmentIds.Count() > 0 ? departmentIds.Contains(c.DepartmentId) : true))
        {

        }
    }
}
