using Application.Contracts.Filters;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class PayrollItemEmployeeSpecs : BaseSpecification<PayrollItemEmployee>
    {
        public PayrollItemEmployeeSpecs(PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
            ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
            AddInclude(i => i.Employee);
        }
        public PayrollItemEmployeeSpecs(int payrollItemId) : base(a => a.PayrollItemId == payrollItemId)
        {
            AddInclude(i => i.Employee);
            AddInclude("Employee.Designation");
            AddInclude("Employee.Department");
        }
    }
}
