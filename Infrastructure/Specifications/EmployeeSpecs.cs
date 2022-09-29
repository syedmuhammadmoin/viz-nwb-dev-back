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
            }
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

        public EmployeeSpecs(bool isEmployeeBP) : base(x => isEmployeeBP == true && x.BusinessPartner.BusinessPartnerType == BusinessPartnerType.Employee)
        {
        }
    }
}
