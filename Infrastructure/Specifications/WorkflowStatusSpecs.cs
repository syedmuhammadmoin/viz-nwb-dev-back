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
    public class WorkFlowStatusSpecs : BaseSpecification<WorkFlowStatus>
    {
        public WorkFlowStatusSpecs(TransactionFormFilter filter, bool isTotalRecord)
            : base(c => (c.Type != StatusType.PreDefined && c.IsDelete == false)
            && (c.Status.Contains(filter.Name != null ? filter.Name : "")))
        {
            if (!isTotalRecord)
            {
                var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
                ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
                ApplyOrderByDescending(i => i.Id);
                
            }
        }

        public WorkFlowStatusSpecs() : base(a => a.Type != StatusType.PreDefined && a.IsDelete == false)
        {
            
        }
    }
}
