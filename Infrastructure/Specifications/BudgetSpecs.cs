using Application.Contracts.Filters;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class BudgetSpecs : BaseSpecification<BudgetMaster>
    {
        public BudgetSpecs(PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
            ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
        }
        public BudgetSpecs(bool forEdit)
        {
            if (forEdit)
            {
                AddInclude(i => i.BudgetLines);
            }
            else
            {
                AddInclude("BudgetLines.Account");
            }
        }
        public BudgetSpecs(DateTime date) : base(a => date >= a.From && date <= a.To)
        {
        }

        public BudgetSpecs(string budgetName) : base (a => budgetName == a.BudgetName)
        {
            AddInclude(i => i.BudgetLines);
            AddInclude("BudgetLines.Account");

        }
    }
}
