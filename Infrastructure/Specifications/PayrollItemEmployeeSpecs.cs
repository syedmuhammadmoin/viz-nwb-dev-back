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
        public PayrollItemEmployeeSpecs(int id, bool isPayrollItem)
            : base(isPayrollItem ? a => a.PayrollItemId == id
            : a => a.EmployeeId == id)
        {

            if (isPayrollItem)
            {
                AddInclude(i => i.PayrollItem);
                AddInclude(i => i.Employee);
            }
            else
            {
                AddInclude(i => i.PayrollItem);
                AddInclude(i => i.Employee);
                AddInclude("Employee.Designation");
                AddInclude("Employee.Department");
            }
        }

    }
}
