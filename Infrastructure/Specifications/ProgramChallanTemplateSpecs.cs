using Application.Contracts.Filters;
using Domain.Entities;

namespace Infrastructure.Specifications
{
    public class ProgramChallanTemplateSpecs : BaseSpecification<ProgramChallanTemplateMaster>
    {
        public ProgramChallanTemplateSpecs(TransactionFormFilter filter, bool isTotalRecord)
        {
            if (!isTotalRecord)
            {
                var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
                ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
                ApplyOrderByDescending(i => i.Id);
                AddInclude(i => i.Program);
                AddInclude(i => i.Campus);
                AddInclude(i => i.Shift);
                AddInclude(i => i.Semester);
                AddInclude(i => i.BankAccount);
            }
        }

        public ProgramChallanTemplateSpecs(bool forEdit)
        {
            if (forEdit)
            {
                AddInclude(i => i.ProgramChallanTemplateLines);
            }
            else
            {
                AddInclude(i => i.Program);
                AddInclude(i => i.Campus);
                AddInclude(i => i.Shift);
                AddInclude(i => i.Semester);
                AddInclude(i => i.BankAccount);
                AddInclude("ProgramChallanTemplateLines.FeeItem");
            }
        }
    }
}
