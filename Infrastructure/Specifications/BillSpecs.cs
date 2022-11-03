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
        public BillSpecs(List<DateTime?> docDate, List<DateTime?> dueDate,
            List<DocumentStatus?> states, TransactionFormFilter filter, bool isTotalRecord) : base(x => (docDate.Count() > 0 ? docDate.Contains(x.BillDate) : true)
            && (dueDate.Count() > 0 ? dueDate.Contains(x.DueDate) : true)
            && x.DocNo.Contains(filter.DocNo != null ? filter.DocNo : "")
            && x.Vendor.Name.Contains(filter.BusinessPartner != null ? filter.BusinessPartner : "")
            && (states.Count() > 0 ? states.Contains(x.Status.State) : true))
        {
            if (!isTotalRecord)
            {
                var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
                ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
                ApplyOrderByDescending(i => i.Id);
                AddInclude(i => i.Vendor);
                AddInclude(i => i.PayableAccount);
                AddInclude(i => i.Status);
                AddInclude(i => i.Campus);
                AddInclude(i => i.GRN);
                AddInclude("BillLines.Account");
                AddInclude("BillLines.Warehouse");
                AddInclude("BillLines.Item");
            }
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
                AddInclude(i => i.GRN);
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
        public BillSpecs(int grnId, bool isGRN) : base(p => isGRN ? p.GRNId == grnId : true )
        {
        }

        //For aging report
        public BillSpecs(string abc)
            : base(c => c.Status.State == DocumentStatus.Unpaid || c.Status.State == DocumentStatus.Partial)
        {
            AddInclude(i => i.Vendor);
        }
    }
}
