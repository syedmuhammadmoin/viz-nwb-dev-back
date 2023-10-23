using Application.Contracts.Filters;
using Domain.Entities;

namespace Infrastructure.Specifications
{
    public class ProgramSpecs : BaseSpecification<Program>
    {
        public ProgramSpecs(TransactionFormFilter filter, bool isTotalRecord)
           : base(c => c.Name.Contains(filter.Name != null ? filter.Name : ""))
        {
            if (!isTotalRecord)
            {
                var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
                ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
                ApplyOrderByDescending(i => i.Id);
                AddInclude(i => i.Degree);
                AddInclude(i => i.AcademicDepartment);
            }
        }

        public ProgramSpecs(bool forEdit)
        {
            if (forEdit)
            {
                AddInclude(i => i.SemesterCourseList);
            }
            else
            {
                AddInclude(i => i.Degree);
                AddInclude(i => i.AcademicDepartment);
                AddInclude("SemesterCourseList.Course");
            }
        }
    }
}
