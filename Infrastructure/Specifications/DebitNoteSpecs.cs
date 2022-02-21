using Application.Contracts.Filters;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class DebitNoteSpecs : BaseSpecification<DebitNoteMaster>
    {
        public DebitNoteSpecs(PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
            ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
            AddInclude(i => i.Vendor);
            AddInclude("DebitNoteLines.Account");
            AddInclude("DebitNoteLines.Location");
            AddInclude("DebitNoteLines.Item");
        }

        public DebitNoteSpecs(bool forEdit)
        {
            if (forEdit)
            {
                AddInclude(i => i.DebitNoteLines);
            }
            else
            {
                AddInclude(i => i.Vendor);
                AddInclude("DebitNoteLines.Account");
                AddInclude("DebitNoteLines.Location");
                AddInclude("DebitNoteLines.Item");
            }

        }
    }
}
