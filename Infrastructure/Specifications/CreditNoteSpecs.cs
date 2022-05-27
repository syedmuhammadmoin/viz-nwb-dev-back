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
            ApplyOrderByDescending(i => i.Id);
            AddInclude(i => i.Customer);
            AddInclude(i => i.ReceivableAccount);
            AddInclude(i => i.Status);
            AddInclude(i => i.Campus);
            AddInclude("CreditNoteLines.Account");
            AddInclude("CreditNoteLines.Warehouse");
            AddInclude("CreditNoteLines.Item");
        }

        public CreditNoteSpecs(bool forEdit)
        {
            if (forEdit)
            {
                AddInclude(i => i.CreditNoteLines);
                AddInclude(i => i.Status);
            }
            else
            {
                AddInclude(i => i.Customer);
                AddInclude(i => i.Status);
                AddInclude(i => i.ReceivableAccount);
                AddInclude(i => i.Campus);
                AddInclude("CreditNoteLines.Account");
                AddInclude("CreditNoteLines.Warehouse");
                AddInclude("CreditNoteLines.Item");
            }
        }

        public CreditNoteSpecs(int transactionId) :
           base(p => (p.Status.State == DocumentStatus.Unpaid
            || p.Status.State == DocumentStatus.Partial) && (p.TransactionId == transactionId))
        {

            AddInclude(i => i.Status);
        }
        public CreditNoteSpecs() : base(e => (e.Status.State != DocumentStatus.Unpaid && e.Status.State != DocumentStatus.Partial && e.Status.State != DocumentStatus.Paid && e.Status.State != DocumentStatus.Draft && e.Status.State != DocumentStatus.Cancelled))
        {
            AddInclude(i => i.Status);
        }
    }
}
