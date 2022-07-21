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
        public InvoiceSpecs(List<DateTime?> docDate, List<DateTime?> dueDate,
            List<DocumentStatus?> states, TransactionFormFilter filter) : base(x => (docDate.Count() > 0 ? docDate.Contains(x.InvoiceDate) : true) && (dueDate.Count() > 0 ? dueDate.Contains(x.DueDate) : true)
            && x.DocNo.Contains(filter.DocNo != null ? filter.DocNo : "")
            && x.Customer.Name.Contains(filter.BusinessPartner != null ? filter.BusinessPartner : "")
            && (states.Count() > 0 ? states.Contains(x.Status.State) : true))
        {
            var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
            ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
            ApplyOrderByDescending(i => i.Id);
            AddInclude(i => i.Customer);
            AddInclude(i => i.ReceivableAccount);
            AddInclude(i => i.Campus);
            AddInclude(i => i.Status);
            AddInclude("InvoiceLines.Account");
            AddInclude("InvoiceLines.Warehouse");
            AddInclude("InvoiceLines.Item");
        }

        public InvoiceSpecs(bool forEdit)
        {
            if (forEdit)
            {
                AddInclude(i => i.InvoiceLines);
                AddInclude(i => i.Status);
            }
            else
            {
                AddInclude(i => i.Customer);
                AddInclude(i => i.Status);
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
        public InvoiceSpecs() : base(e => (e.Status.State != DocumentStatus.Unpaid && e.Status.State != DocumentStatus.Partial && e.Status.State != DocumentStatus.Paid && e.Status.State != DocumentStatus.Draft && e.Status.State != DocumentStatus.Cancelled))
        {
            AddInclude(i => i.Status);
        }
        
        //For aging report
        public InvoiceSpecs(string abc)
            : base(c => c.Status.State == DocumentStatus.Unpaid || c.Status.State == DocumentStatus.Partial)
        {
            AddInclude(i => i.Customer);
            ApplyAsNoTracking();
        }
    }
}
