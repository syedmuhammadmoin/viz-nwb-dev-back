using Application.Contracts.Filters;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class WarehouseSpecs : BaseSpecification<Warehouse>
    {
        public WarehouseSpecs(TransactionFormFilter filter, bool isTotalRecord)
            : base(c => c.Name.Contains(filter.Name != null ? filter.Name : ""))
        {
            if (!isTotalRecord)
            {
                var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
                ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
                ApplyOrderByDescending(i => i.Id);
                AddInclude(i => i.Campus);
                
            }
        }

        public WarehouseSpecs(int campusId) : base(x => x.CampusId == campusId)
        {
            
        }
        public WarehouseSpecs() 
        {
              AddInclude(i => i.Campus);
        }
    }
}
