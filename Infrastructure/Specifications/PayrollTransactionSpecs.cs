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
    public class PayrollTransactionSpecs : BaseSpecification<PayrollTransactionMaster>
    {
        public PayrollTransactionSpecs(PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
            ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
            ApplyOrderByDescending(i => i.Id);
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

        public PayrollTransactionSpecs(int month, int year, int?[] departmentIds) 
            : base(x => x.Month == month && x.Year == year
                && (departmentIds.Count() > 0 ? departmentIds.Contains(x.DepartmentId) : true)
                && (x.Status.State == DocumentStatus.Draft || x.Status.State == DocumentStatus.Rejected))
        {

        }

        public PayrollTransactionSpecs(int month, int year, int?[] departmentIds, string getPayrollPayment) 
            : base(x => x.Month == month && x.Year == year
                && (departmentIds.Count() > 0 ? departmentIds.Contains(x.DepartmentId) : true)
                && (x.Status.State == DocumentStatus.Unpaid)
                && (x.TransactionId != null || x.TransactionId != 0)
            )
        {
            AddInclude(i => i.Status);
        }

        public PayrollTransactionSpecs(int month, int year, int?[] departmentIds, bool isPayrollTransactoin)
            : base(x => x.Month == month && x.Year == year
                && (departmentIds.Count() > 0 ? departmentIds.Contains(x.DepartmentId) : true)
                && (isPayrollTransactoin ? (x.Status.State == DocumentStatus.Draft || x.Status.State == DocumentStatus.Rejected) : true)
            )
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

        public PayrollTransactionSpecs(List<int?> months, List<int?> years, List<int?> employees,
            DateTime fromDate, DateTime toDate, string designation, string department, string bps)
            : base(x =>
                ((((DateTime)x.ModifiedDate).Date >= fromDate && ((DateTime)x.ModifiedDate).Date <= toDate))
                && (months.Count() > 0 ? months.Contains(x.Month) : true)
                && (years.Count() > 0 ? years.Contains(x.Year) : true)
                && (employees.Count() > 0 ? employees.Contains(x.EmployeeId) : true)
                && x.Designation.Name.Contains(designation != null ? designation : "")
                && x.Department.Name.Contains(department != null ? department : "")
                && x.BPSName.Contains(bps != null ? bps : "")
                && (x.Status.State != DocumentStatus.Draft && x.Status.State != DocumentStatus.Cancelled && x.Status.State != DocumentStatus.Rejected)
                )
        {
            AddInclude(a => a.AccountPayable);
            AddInclude(a => a.Department);
            AddInclude(a => a.Designation);
            AddInclude(a => a.Employee);
            AddInclude(i => i.Status);
            AddInclude("PayrollTransactionLines.PayrollItem");
            AddInclude("PayrollTransactionLines.Account");
            ApplyOrderByDescending(i => i.Id);
        }
    }
}
