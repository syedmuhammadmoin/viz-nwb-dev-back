using Application.Contracts.Filters;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class TaxGroupSpecs : BaseSpecification<TaxGroup>
    {
        public TaxGroupSpecs(TransactionFormFilter filter, bool isTotalRecord)
          : base(c => c.Name.Contains(filter.Name != null ? filter.Name : ""))
        {
            if (!isTotalRecord)
            {
                var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
                ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
                ApplyOrderByDescending(i => i.Id);
                AddInclude(i => i.AdvanceAccount);
                AddInclude(i => i.PayableAccount);
                AddInclude(i => i.ReceivableAccount);
                AddInclude(i => i.Country);
            }
        }
        public TaxGroupSpecs(bool isEdit)
        {
            AddInclude(i => i.AdvanceAccount);
            AddInclude(i => i.PayableAccount);
            AddInclude(i => i.ReceivableAccount);
            AddInclude(i => i.Country);
        }
    }
}
