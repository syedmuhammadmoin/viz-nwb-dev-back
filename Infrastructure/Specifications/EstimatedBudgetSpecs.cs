using Application.Contracts.Filters;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class EstimatedBudgetSpecs : BaseSpecification<EstimatedBudgetMaster>
    {
        public EstimatedBudgetSpecs(TransactionFormFilter filter, bool isTotalRecord)
            : base(c => c.EstimatedBudgetName.Contains(filter.Name != null ? filter.Name : ""))
        {
            if (!isTotalRecord)
            {
                var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
                ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
                AddInclude(i => i.PreviousBudget);
                ApplyOrderByDescending(i => i.Id);
                ApplyAsNoTracking();
            }
        }
            public EstimatedBudgetSpecs(bool forEdit)
            {
                if (forEdit)
                {
                    AddInclude(i => i.PreviousBudget);
                    AddInclude(i => i.EstimatedBudgetLines);
                }
                else
                {
                    AddInclude(i => i.PreviousBudget);
                    AddInclude("EstimatedBudgetLines.Account");
                    ApplyAsNoTracking();
                }
            }
        }
    }
