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
    public class EmployeeSpecs : BaseSpecification<Employee>
    {
        public EmployeeSpecs(TransactionFormFilter filter, bool isTotalRecord)
            : base(c => c.Name.Contains(filter.Name != null ? filter.Name : "")
                && c.CNIC.Contains(filter.DocNo != null ? filter.DocNo : "")
                && c.BankName.Contains(filter.BusinessPartner != null ? filter.BusinessPartner : "")
                && c.Department.Name.Contains(filter.Department != null ? filter.Department : "")
                && c.Designation.Name.Contains(filter.Designation != null ? filter.Designation : "")
                && (filter.IsActive != null ? c.isActive == filter.IsActive : true))
        {
            if (!isTotalRecord)
            {
                var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
                ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
                ApplyOrderByDescending(i => i.Id);
                AddInclude(i => i.Department);
                AddInclude(i => i.Designation);
                AddInclude("Department.Campus");
            }
        }

        public EmployeeSpecs(bool forEdit)
        {
            if (forEdit)
            {
                AddInclude(i => i.BusinessPartner);
            }
            else
            {
                AddInclude("BusinessPartner.AccountPayable");
                AddInclude(i => i.Department);
                AddInclude(i => i.Designation);
                AddInclude("Department.Campus");
            }
        }

        public EmployeeSpecs(string getCNIC) : base(e => e.CNIC == getCNIC)
        {
            AddInclude(i => i.Department);
            AddInclude(i => i.BusinessPartner);
            AddInclude(i => i.Designation);
            AddInclude("BusinessPartner.AccountPayable");
            AddInclude("Department.Campus");
        }

        public EmployeeSpecs(bool isActive, int?[] departmentIds)
            : base(c => c.isActive == isActive &&
                (departmentIds.Count() > 0 ? departmentIds.Contains(c.DepartmentId) : true))
        {

        }

        public EmployeeSpecs() : base(x => x.BusinessPartner.BusinessPartnerType == BusinessPartnerType.Employee)
        {
        }
    }
}
