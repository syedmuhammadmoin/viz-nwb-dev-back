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
        public PayrollTransactionSpecs(List<DateTime?> docDate,
            List<DocumentStatus?> states, TransactionFormFilter filter, bool isTotalRecord)
            : base(c => (docDate.Count() > 0 ? docDate.Contains(c.TransDate) : true)
                && c.DocNo.Contains(filter.DocNo != null ? filter.DocNo : "")
                && c.Employee.Name.Contains(filter.BusinessPartner != null ? filter.BusinessPartner : "")
                && c.Department.Name.Contains(filter.Department != null ? filter.Department : "")
                && c.Designation.Name.Contains(filter.Designation != null ? filter.Designation : "")
                && (states.Count() > 0 ? states.Contains(c.Status.State) : true))
        {
            if (!isTotalRecord)
            {
                var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
                ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
                ApplyOrderByDescending(i => i.Id);
                AddInclude(a => a.AccountPayable);
                AddInclude(a => a.Department);
                AddInclude(a => a.Designation);
                AddInclude(a => a.Campus);
                AddInclude(a => a.PayrollTransactionLines);
                AddInclude(a => a.Status);
                AddInclude(a => a.Employee);
                AddInclude("PayrollTransactionLines.PayrollItem");
                AddInclude("PayrollTransactionLines.Account");
            }
        }

        public PayrollTransactionSpecs(bool forEdit)
        {
            if (forEdit)
            {
                AddInclude(i => i.Status);
                AddInclude(a => a.Employee);
                AddInclude("PayrollTransactionLines.PayrollItem");
                AddInclude("PayrollTransactionLines.Account");
            }
            else
            {
                AddInclude(a => a.AccountPayable);
                AddInclude(a => a.Department);
                AddInclude(a => a.Designation);
                AddInclude(a => a.Campus);
                AddInclude(a => a.Employee);
                AddInclude(i => i.PayrollTransactionLines);
                AddInclude(i => i.Status);
                AddInclude("PayrollTransactionLines.PayrollItem");
                AddInclude("PayrollTransactionLines.Account");
            }

        }

        public PayrollTransactionSpecs(int month, int year, int empId) : base(x => x.EmployeeId == empId && x.Year == year && x.Month == month)
        {
            AddInclude(a => a.Employee);
            AddInclude("PayrollTransactionLines.PayrollItem");
            AddInclude("PayrollTransactionLines.Account");
        }

        public PayrollTransactionSpecs(int month, int year, int?[] departmentIds, int campusId) 
            : base(x => x.Month == month && x.Year == year
                && (departmentIds.Count() > 0 ? departmentIds.Contains(x.DepartmentId) : true)
                && (x.CampusId == campusId && x.Campus.IsActive == true)
                && (x.Status.State == DocumentStatus.Draft || x.Status.State == DocumentStatus.Rejected))
        {

        }

        public PayrollTransactionSpecs(int month, int year, int?[] departmentIds,int campusId, string getPayrollPayment) 
            : base(x => x.Month == month && x.Year == year
                && (departmentIds.Count() > 0 ? departmentIds.Contains(x.DepartmentId) : true)
                && (x.Status.State == DocumentStatus.Unpaid)
                && (x.CampusId == campusId && x.Campus.IsActive == true)
                && (x.TransactionId != null || x.TransactionId != 0)
            )
        {
            AddInclude(a => a.AccountPayable);
            AddInclude(a => a.Department);
            AddInclude(a => a.Designation);
            AddInclude(a => a.Campus);
            AddInclude(a => a.Employee);
            AddInclude(i => i.PayrollTransactionLines);
            AddInclude(i => i.Status);
            AddInclude("PayrollTransactionLines.PayrollItem");
            AddInclude("PayrollTransactionLines.Account");
        }

        public PayrollTransactionSpecs(int month, int year, int?[] departmentIds, int campusId, bool isPayrollTransactoin)
            : base(x => x.Month == month && x.Year == year
                && (departmentIds.Count() > 0 ? departmentIds.Contains(x.DepartmentId) : true)
                && (x.CampusId == campusId && x.Campus.IsActive == true)
                && (isPayrollTransactoin ? (x.Status.State == DocumentStatus.Draft || x.Status.State == DocumentStatus.Rejected) : true)
            )
        {
            AddInclude(a => a.AccountPayable);
            AddInclude(a => a.Department);
            AddInclude(a => a.Designation);
            AddInclude(a => a.Campus);
            AddInclude(a => a.Employee);
            AddInclude(i => i.PayrollTransactionLines);
            AddInclude(i => i.Status);
            AddInclude("PayrollTransactionLines.PayrollItem");
            AddInclude("PayrollTransactionLines.Account");
        }

        public PayrollTransactionSpecs(List<int?> months, List<int?> years, List<int?> employees,
            DateTime fromDate, DateTime toDate, string designation, string department, string campus, string bps)
            : base(x =>
                ((((DateTime)x.ModifiedDate).Date >= fromDate && ((DateTime)x.ModifiedDate).Date <= toDate))
                && (months.Count() > 0 ? months.Contains(x.Month) : true)
                && (years.Count() > 0 ? years.Contains(x.Year) : true)
                && (employees.Count() > 0 ? employees.Contains(x.EmployeeId) : true)
                && x.Designation.Name.Contains(designation != null ? designation : "")
                && x.Department.Name.Contains(department != null ? department : "")
                && x.Campus.Name.Contains(campus != null ? campus : "")
                && x.BPSName.Contains(bps != null ? bps : "")
                && (x.Status.State != DocumentStatus.Draft && x.Status.State != DocumentStatus.Cancelled && x.Status.State != DocumentStatus.Rejected)
                )
        {
            AddInclude(a => a.AccountPayable);
            AddInclude(a => a.Department);
            AddInclude(a => a.Designation);
            AddInclude(a => a.Campus);
            AddInclude(a => a.Employee);
            AddInclude(i => i.Status);
            AddInclude("PayrollTransactionLines.PayrollItem");
            AddInclude("PayrollTransactionLines.Account");
            ApplyOrderByDescending(i => i.Id);
        }

        public PayrollTransactionSpecs(int transactionId) :
         base(p => (p.Status.State == DocumentStatus.Unpaid
          || p.Status.State == DocumentStatus.Partial) && (p.TransactionId == transactionId))
        {
            AddInclude(i => i.Status);
        }

        public PayrollTransactionSpecs(int?[] months, int year, List<int?> campus) 
            : base(x =>
            (months.Count() > 0 ? months.Contains(x.Month) : true)
            && (campus.Count() > 0 ? campus.Contains(x.CampusId) : true)
            && (x.Status.State == DocumentStatus.Unpaid || x.Status.State == DocumentStatus.Paid || x.Status.State == DocumentStatus.Partial)
            && x.Year == year
            )
        {
            AddInclude(i => i.PayrollTransactionLines);
            AddInclude("PayrollTransactionLines.Account"); 
        }
        public PayrollTransactionSpecs(int month, int year, List<int?> campuses)
            : base(x =>
            (x.Month == month)
            && x.Year == year
            && (campuses.Count() > 0 ? campuses.Contains(x.CampusId) : true)
            && (x.Status.State == DocumentStatus.Unpaid || x.Status.State == DocumentStatus.Paid || x.Status.State == DocumentStatus.Partial)
            )
        {
            AddInclude(a => a.Employee);
            
        }
    }
}
