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
    public class PayrollItemEmployeeSpecs : BaseSpecification<PayrollItemEmployee>
    {
        public PayrollItemEmployeeSpecs(PaginationFilter filter, bool isTotalRecord)
        {
            if (!isTotalRecord)
            {
                var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
                ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
                AddInclude(i => i.Employee);
                ApplyOrderByDescending(i => i.Id);
            }
        }
            public PayrollItemEmployeeSpecs(int id, bool isPayrollItem)
            : base(isPayrollItem ? a => a.PayrollItemId == id
            : a => a.EmployeeId == id)
        {
                AddInclude(i => i.PayrollItem);
                AddInclude(i => i.Employee);
                AddInclude("PayrollItem.Account");
                AddInclude("Employee.Designation");
                AddInclude("Employee.Department");
                ApplyAsNoTracking();
            }

            public PayrollItemEmployeeSpecs(int empId, PayrollType payrollType)
            : base(a => a.EmployeeId == empId && a.PayrollType == payrollType)
            {
                  ApplyAsNoTracking();
        }

        }
    }
