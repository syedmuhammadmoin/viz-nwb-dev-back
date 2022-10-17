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
        public WorkFlowSpecs(TransactionFormFilter filter, bool isTotalRecord)
            : base(c => c.Name.Contains(filter.Name != null ? filter.Name : ""))
        {
            if (!isTotalRecord)
            {
                var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
                ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
                ApplyOrderByDescending(i => i.Id);
                AddInclude("WorkflowTransitions.CurrentStatus");
                AddInclude("WorkflowTransitions.NextStatus");
                ApplyAsNoTracking();
            }
        }
        public WorkFlowSpecs(bool forEdit)
        {
            if (forEdit)
            {
                AddInclude(i => i.WorkflowTransitions);
            }
            else
            {
                AddInclude("WorkflowTransitions.CurrentStatus");
                AddInclude("WorkflowTransitions.NextStatus");
                AddInclude("WorkflowTransitions.AllowedRole");
                ApplyAsNoTracking();
            }
        }

        public WorkFlowSpecs(DocType docType) : base(e => (e.DocType == docType) && (e.IsActive == true))
        {
            AddInclude("WorkflowTransitions.AllowedRole");
            AddInclude("WorkflowTransitions.CurrentStatus");
            AddInclude("WorkflowTransitions.NextStatus");
            ApplyAsNoTracking();
        }

        public WorkFlowSpecs(DocType docType, int id) : base(e => (e.DocType == docType) && (e.IsActive == true) && (e.Id != id))
        {
            ApplyAsNoTracking();
        }
    }
}
