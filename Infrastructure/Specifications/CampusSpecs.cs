using Application.Contracts.Filters;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class CampusSpecs : BaseSpecification<Campus>
    {
        public CampusSpecs(TransactionFormFilter filter) 
            : base(c => c.Name.Contains(filter.BusinessPartner != null ? filter.BusinessPartner : ""))
        {
            var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
            ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
            ApplyOrderByDescending(i => i.Id);
        }
    }
}
