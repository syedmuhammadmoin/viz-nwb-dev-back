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
    public class BudgetReappropriationSpecs : BaseSpecification<BudgetReappropriationMaster>
    {
        public BudgetReappropriationSpecs(TransactionFormFilter filter, bool isTotalRecord)
            : base(c => c.Budget.BudgetName.Contains(filter.Name != null ? filter.Name : ""))
        {
            if (!isTotalRecord)
            {
                var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
                ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
                ApplyOrderByDescending(i => i.Id);
                AddInclude(i => i.BudgetReappropriationLines);
                AddInclude(i => i.Budget);
                AddInclude(i => i.Status);
                AddInclude("BudgetReappropriationLines.Campus");
                AddInclude("BudgetReappropriationLines.Level4");
            }
        }
        public BudgetReappropriationSpecs(bool forEdit)
        {
            if (forEdit)
            {
                AddInclude(i => i.BudgetReappropriationLines);
                AddInclude(i => i.Status);
            }
            else
            {
                AddInclude(i => i.Status);
                AddInclude(i => i.Budget);
                AddInclude(i => i.BudgetReappropriationLines);
                AddInclude("BudgetReappropriationLines.Campus");
                AddInclude("BudgetReappropriationLines.Level4");
            }
        }
        public BudgetReappropriationSpecs(string workflow)
          : base(e => (e.Status.State != DocumentStatus.Unpaid && e.Status.State != DocumentStatus.Partial && e.Status.State != DocumentStatus.Paid && e.Status.State != DocumentStatus.Draft && e.Status.State != DocumentStatus.Cancelled))
        {
            AddInclude(i => i.Status);
        }
    }
}
