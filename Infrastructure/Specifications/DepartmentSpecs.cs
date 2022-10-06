using Application.Contracts.Filters;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class DepartmentSpecs : BaseSpecification<Department>
    {
        public DepartmentSpecs(TransactionFormFilter filter, bool isTotalRecord)
            : base(c => c.Name.Contains(filter.Name != null ? filter.Name : ""))
        {
            if (!isTotalRecord)
            {
                var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
                ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
                AddInclude(i => i.Campus);
                ApplyOrderByDescending(i => i.Id);
            }
        }
        public DepartmentSpecs()
        {
            AddInclude(i => i.Campus);
        }
        public DepartmentSpecs(int campusId) : base(x => x.CampusId == campusId && x.Campus.IsActive == true)
        {

        }
    }
}
