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
            AddInclude(i=> i.Vendor);
            AddInclude("BillLines.Account");
            AddInclude("BillLines.Location");
            AddInclude("BillLines.Item");
        }
        public BillSpecs(bool forEdit)
        {
            if (forEdit)
            {
                AddInclude(i => i.BillLines);
            }
            else
            {
                AddInclude(i=> i.Vendor);
                AddInclude("BillLines.Account");
                AddInclude("BillLines.Location");
                AddInclude("BillLines.Item");
            }
        }
        public BillSpecs(int transactionId) :
         base(p => (p.Status.State == DocumentStatus.Unpaid
          || p.Status.State == DocumentStatus.Partial) && (p.TransactionId == transactionId))
        {

            AddInclude(i => i.Status);
        }
    }
}
