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
    public class DebitNoteSpecs : BaseSpecification<DebitNoteMaster>
    {
        public DebitNoteSpecs(PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
            ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
            ApplyOrderByDescending(i => i.Id);
            AddInclude(i => i.Vendor);
            AddInclude(i => i.PayableAccount);
            AddInclude(i => i.Campus);
            AddInclude(i => i.Status);
            AddInclude("DebitNoteLines.Account");
            AddInclude("DebitNoteLines.Warehouse");
            AddInclude("DebitNoteLines.Item");
        }

        public DebitNoteSpecs(bool forEdit)
        {
            if (forEdit)
            {
                AddInclude(i => i.DebitNoteLines);
                AddInclude(i => i.Status);
            }
            else
            {
                AddInclude(i => i.Vendor);
                AddInclude(i => i.PayableAccount);
                AddInclude(i => i.Campus);
                AddInclude(i => i.Status);
                AddInclude("DebitNoteLines.Account");
                AddInclude("DebitNoteLines.Warehouse");
                AddInclude("DebitNoteLines.Item");
            }
        }
        public DebitNoteSpecs(int transactionId) :
           base(p => (p.Status.State == DocumentStatus.Unpaid
            || p.Status.State == DocumentStatus.Partial) && (p.TransactionId == transactionId))
        {

            AddInclude(i => i.Status);
        }
        public DebitNoteSpecs() : base(e => (e.Status.State != DocumentStatus.Unpaid && e.Status.State != DocumentStatus.Partial && e.Status.State != DocumentStatus.Paid && e.Status.State != DocumentStatus.Draft && e.Status.State != DocumentStatus.Cancelled))
        {
            AddInclude(i => i.Status);
        }
    }
}
