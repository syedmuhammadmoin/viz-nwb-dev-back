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
        public BudgetSpecs(TransactionFormFilter filter, bool isTotalRecord)
            : base(c => c.BudgetName.Contains(filter.Name != null ? filter.Name : ""))
        {
            if (!isTotalRecord)
            {
                var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
                ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
                ApplyOrderByDescending(i => i.Id);
                AddInclude(i => i.Campus);
            }
        }

        public BudgetSpecs(bool forEdit)
        {
            if (forEdit)
            {
                AddInclude(i => i.BudgetLines);
            }
            else
            {
                AddInclude(i => i.Campus);
                AddInclude("BudgetLines.Account");
            }
        }
        public BudgetSpecs(DateTime date, int campusId) : base(a => (date >= a.From && date <= a.To) && campusId == a.CampusId)
        {
        }

        public BudgetSpecs(string budgetName) : base(a => budgetName == a.BudgetName)
        {
            AddInclude(i => i.Campus);
            AddInclude(i => i.BudgetLines);
            AddInclude("BudgetLines.Account");

        }
    }
}
