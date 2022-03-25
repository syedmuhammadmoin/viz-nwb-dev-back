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
    public class InvoiceSpecs : BaseSpecification<InvoiceMaster>
    {
        public InvoiceSpecs(PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
            ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
            AddInclude(i => i.Customer);
            AddInclude(i => i.ReceivableAccount);
            AddInclude(i => i.Campus);
            AddInclude("InvoiceLines.Account");
            AddInclude("InvoiceLines.Warehouse");
            AddInclude("InvoiceLines.Item");
        }

        public InvoiceSpecs(bool forEdit)
        {
            if (forEdit)
            {
                AddInclude(i => i.InvoiceLines);
            }
            else
            {
                AddInclude(i => i.Customer);
                AddInclude(i => i.Campus);
                AddInclude(i => i.ReceivableAccount); 
                AddInclude("InvoiceLines.Account");
                AddInclude("InvoiceLines.Warehouse");
                AddInclude("InvoiceLines.Item");
            }
        }

        public InvoiceSpecs(int transactionId) :
          base(p => (p.Status.State == DocumentStatus.Unpaid
           || p.Status.State == DocumentStatus.Partial) && (p.TransactionId == transactionId))
        {

            AddInclude(i => i.Status);
        }
    }
}
