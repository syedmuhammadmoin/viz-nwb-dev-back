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
    public class JournalEntrySpecs : BaseSpecification<JournalEntryMaster>
    {
        public JournalEntrySpecs(PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
            ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
            AddInclude(i => i.Campus);
            AddInclude(i => i.Status);
            ApplyOrderByDescending(i => i.Id);
            AddInclude("JournalEntryLines.BusinessPartner");
            AddInclude("JournalEntryLines.Account");
            AddInclude("JournalEntryLines.Warehouse");
        }

        public JournalEntrySpecs(bool forEdit)
        {
            if (forEdit)
            {
                AddInclude(i => i.JournalEntryLines);
                AddInclude(i => i.Status);
            }
            else
            {
                AddInclude(i => i.Campus);
                AddInclude(i => i.Status);
                AddInclude("JournalEntryLines.BusinessPartner");
                AddInclude("JournalEntryLines.Account");
                AddInclude("JournalEntryLines.Warehouse");
            }
        }
        public JournalEntrySpecs() : base(e => (e.Status.State != DocumentStatus.Unpaid && e.Status.State != DocumentStatus.Partial && e.Status.State != DocumentStatus.Paid && e.Status.State != DocumentStatus.Draft && e.Status.State != DocumentStatus.Cancelled))
        {
            AddInclude(i => i.Status);
        }
    }
}
