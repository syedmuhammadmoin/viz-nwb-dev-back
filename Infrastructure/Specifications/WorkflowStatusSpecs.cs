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
        public WorkFlowStatusSpecs(PaginationFilter filter) : base(a => a.Type != StatusType.PreDefined && a.IsDelete == false)
        {
            var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
            ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
        }

        public WorkFlowStatusSpecs() : base(a => a.Type != StatusType.PreDefined && a.IsDelete == false)
        {
        }
    }
}
