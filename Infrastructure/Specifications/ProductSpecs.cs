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
    public class ProductSpecs : BaseSpecification<Product>
    {
        public ProductSpecs(TransactionFormFilter filter)
            : base(c => c.ProductName.Contains(filter.Name != null ? filter.Name : "")
           && (c.Barcode.Contains(filter.BusinessPartner != null ? filter.BusinessPartner : "")
           && (c.Category.Name.Contains(filter.Category != null ? filter.Category : "")
            )))
        {
            var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
            ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
            ApplyOrderByDescending(i => i.Id);
            AddInclude(i => i.Category);
        }
        public ProductSpecs()
        {
            AddInclude(i => i.Category);
        }
    }
}
