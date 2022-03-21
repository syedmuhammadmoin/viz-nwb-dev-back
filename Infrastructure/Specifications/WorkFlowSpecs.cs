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
    public class WorkFlowSpecs : BaseSpecification<WorkFlowMaster>
    {
        public WorkFlowSpecs(PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
            ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
            AddInclude("WorkflowTransitions.WorkFlowStatus");
        }
        public WorkFlowSpecs(bool forEdit)
        {
            if (forEdit)
            {
                AddInclude(i => i.WorkflowTransitions);
            }
            else
            {
                AddInclude("WorkflowTransitions.WorkFlowStatus");
            }
        }

        public WorkFlowSpecs(DocType docType) : base(e => (e.DocType == docType) && (e.IsActive == true))
        {
        }
    }
}
