﻿using Application.Contracts.Filters;
using Domain.Entities;

namespace Infrastructure.Specifications
{
    public class AcademicDepartmentSpecs : BaseSpecification<AcademicDepartment>
    {
        public AcademicDepartmentSpecs(TransactionFormFilter filter, bool isTotalRecord)
           : base(c => c.Name.Contains(filter.Name != null ? filter.Name : ""))
        {
            if (!isTotalRecord)
            {
                var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
                ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
                ApplyOrderByDescending(i => i.Id);
                AddInclude(i => i.Faculty);
            }
        }

        public AcademicDepartmentSpecs()
        {
            AddInclude(i => i.Faculty);
        }
    }
}
