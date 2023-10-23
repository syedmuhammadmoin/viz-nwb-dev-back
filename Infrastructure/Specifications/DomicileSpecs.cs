using Application.Contracts.Filters;
using Domain.Entities;

namespace Infrastructure.Specifications
{
    public class DomicileSpecs : BaseSpecification<Domicile>
    {
        public DomicileSpecs(TransactionFormFilter filter, bool isTotalRecord)
           : base(c => c.Name.Contains(filter.Name != null ? filter.Name : ""))
        {
            if (!isTotalRecord)
            {
                var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
                ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
                ApplyOrderByDescending(i => i.Id);
                AddInclude(i => i.District);
            }
        }

        public DomicileSpecs()
        {
            AddInclude(i => i.District);
        }

        public DomicileSpecs(int districtId) : base (i => i.DistrictId == districtId)
        {
        }
    }
}
