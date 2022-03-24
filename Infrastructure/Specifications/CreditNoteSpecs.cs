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
    public class CreditNoteSpecs : BaseSpecification<CreditNoteMaster>
    {
        public CreditNoteSpecs(PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
            ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
            AddInclude(i => i.Customer);
            AddInclude("CreditNoteLines.Account");
            AddInclude("CreditNoteLines.Location");
            AddInclude("CreditNoteLines.Item");
        }

        public CreditNoteSpecs(bool forEdit)
        {
            if (forEdit)
            {
                AddInclude(i => i.CreditNoteLines);
            }
            else
            {
                AddInclude(i => i.Customer);
                AddInclude("CreditNoteLines.Account");
                AddInclude("CreditNoteLines.Location");
                AddInclude("CreditNoteLines.Item");
            }
        }

        public CreditNoteSpecs(int transactionId) :
           base(p => (p.Status.State == DocumentStatus.Unpaid
            || p.Status.State == DocumentStatus.Partial) && (p.TransactionId == transactionId))
        {

            AddInclude(i => i.Status);
        }
    }
}
