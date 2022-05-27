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
    public class BillSpecs : BaseSpecification<BillMaster>
    {
        public BillSpecs(PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
            ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
            ApplyOrderByDescending(i => i.Id);
            AddInclude(i => i.Vendor);
            AddInclude(i => i.PayableAccount);
            AddInclude(i => i.Status);
            AddInclude(i => i.Campus);
            AddInclude("BillLines.Account");
            AddInclude("BillLines.Warehouse");
            AddInclude("BillLines.Item");
        }
        public BillSpecs(bool forEdit)
        {
            if (forEdit)
            {
                AddInclude(i => i.BillLines);
                AddInclude(i => i.Status);
            }
            else
            {
                AddInclude(i => i.Vendor);
                AddInclude(i => i.Campus);
                AddInclude(i => i.Status);
                AddInclude(i => i.PayableAccount);
                AddInclude("BillLines.Account");
                AddInclude("BillLines.Warehouse");
                AddInclude("BillLines.Item");
            }
        }
        public BillSpecs(int transactionId) :
         base(p => (p.Status.State == DocumentStatus.Unpaid
          || p.Status.State == DocumentStatus.Partial) && (p.TransactionId == transactionId))
        {

            AddInclude(i => i.Status);
        }
        public BillSpecs() : base(e => (e.Status.State != DocumentStatus.Unpaid && e.Status.State != DocumentStatus.Partial && e.Status.State != DocumentStatus.Paid && e.Status.State != DocumentStatus.Draft && e.Status.State != DocumentStatus.Cancelled))
        {
            AddInclude(i => i.Status);
        }
    }
}
