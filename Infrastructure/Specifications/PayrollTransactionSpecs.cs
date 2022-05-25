using Application.Contracts.Filters;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class PayrollTransactionSpecs : BaseSpecification<PayrollTransactionMaster>
    {
        public PayrollTransactionSpecs(PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
            ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
            AddInclude(a => a.AccountPayable);
            AddInclude(a => a.Department);
            AddInclude(a => a.Designation);
            AddInclude(a => a.PayrollTransactionLines);
            AddInclude(a => a.Status);
            AddInclude(a => a.Employee);
            AddInclude("PayrollTransactionLines.PayrollItem");
            AddInclude("PayrollTransactionLines.Account");
        }

        public PayrollTransactionSpecs(bool forEdit)
        {
            if (forEdit)
            {
                AddInclude(i => i.PayrollTransactionLines);
                AddInclude(i => i.Status);
                AddInclude(a => a.Employee);
                AddInclude(i => i.Status);
                AddInclude("PayrollTransactionLines.PayrollItem");
                AddInclude("PayrollTransactionLines.Account");
            }
            else
            {
                AddInclude(a => a.AccountPayable);
                AddInclude(a => a.Department);
                AddInclude(a => a.Designation);
                AddInclude(a => a.Employee);
                AddInclude(i => i.PayrollTransactionLines);
                AddInclude(i => i.Status);
                AddInclude("PayrollTransactionLines.PayrollItem");
                AddInclude("PayrollTransactionLines.Account");
            }

        }

        public PayrollTransactionSpecs(int month, int year, int empId) : base(x => x.EmployeeId == empId && x.Year == year && x.Month == month)
        {

        }
    }
}
