using Application.Contracts.Filters;
using Domain.Entities;

namespace Infrastructure.Specifications
{
    public class BatchSpecs : BaseSpecification<BatchMaster>
    {
        public BatchSpecs(TransactionFormFilter filter, bool isTotalRecord)
            : base(c => c.Name.Contains(filter.Name != null ? filter.Name : ""))
        {
            if (!isTotalRecord)
            {
                var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
                ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
                ApplyOrderByDescending(i => i.Id);
                AddInclude(i => i.Semester);
                AddInclude(i => i.Campus);
                AddInclude(i => i.Shift);
            }
        }

        public BatchSpecs(bool forEdit)
        {
            if (forEdit)
            {
                AddInclude(i => i.BatchLines);
            }
            else
            {
                AddInclude(i => i.Semester);
                AddInclude(i => i.Campus);
                AddInclude(i => i.Shift);
                AddInclude("BatchLines.Program");
                AddInclude("Criteria.Criteria.Qualification");
                AddInclude("Criteria.Criteria.Subject");
            }
        }
    }
}
