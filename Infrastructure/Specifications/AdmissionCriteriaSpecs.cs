using Application.Contracts.Filters;
using Domain.Entities;

namespace Infrastructure.Specifications
{
    public class AdmissionCriteriaSpecs : BaseSpecification<AdmissionCriteria>
    {
        public AdmissionCriteriaSpecs(TransactionFormFilter filter, bool isTotalRecord)
           : base(c => c.Description.Contains(filter.Name != null ? filter.Name : ""))
        {
            if (!isTotalRecord)
            {
                var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
                ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
                ApplyOrderByDescending(i => i.Id);
                AddInclude(i => i.Program);
                AddInclude(i => i.Qualification);
                AddInclude(i => i.Subject);
            }
        }

        public AdmissionCriteriaSpecs()
        {
            AddInclude(i => i.Program);
            AddInclude(i => i.Qualification);
            AddInclude(i => i.Subject);
        }
    }
}
