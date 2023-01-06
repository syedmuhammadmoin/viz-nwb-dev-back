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
    public class RequisitionSpecs : BaseSpecification<RequisitionMaster>
    {
        public RequisitionSpecs(List<DateTime?> docDate,
            List<DocumentStatus?> states, TransactionFormFilter filter, bool isTotalRecord)
            : base(x => (docDate.Count() > 0 ? docDate.Contains(x.RequisitionDate) : true)
            && x.DocNo.Contains(filter.DocNo != null ? filter.DocNo : "")
            && x.Employee.Name.Contains(filter.BusinessPartner != null ? filter.BusinessPartner : "")
            && (states.Count() > 0 ? states.Contains(x.Status.State) : true))
        {
            if (!isTotalRecord)
            {
                var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
                ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
                ApplyOrderByDescending(i => i.Id);
                AddInclude(i => i.Campus);
                AddInclude(i => i.Status);
                AddInclude(i => i.Employee);
                AddInclude("RequisitionLines.Item");
                AddInclude("RequisitionLines.Warehouse");
            }
        }

        public RequisitionSpecs(bool forEdit)
        {
            if (forEdit)
            {
                AddInclude(i => i.RequisitionLines);
                AddInclude(i => i.Status);
            }
            else
            {
                AddInclude(i => i.Campus);
                AddInclude(i => i.Status);
                AddInclude(i => i.Employee);
                AddInclude("RequisitionLines.Item");
                AddInclude("RequisitionLines.Warehouse");
            }
        }
        public RequisitionSpecs()
        : base(x => x.Status.State != DocumentStatus.Paid)
        {
            AddInclude(i => i.RequisitionLines);
        }
        public RequisitionSpecs(string workflow)
         : base(e => (e.Status.State != DocumentStatus.Unpaid && e.Status.State != DocumentStatus.Partial && e.Status.State != DocumentStatus.Paid && e.Status.State != DocumentStatus.Draft && e.Status.State != DocumentStatus.Cancelled))
        {
            AddInclude(i => i.Status);
        }

        public RequisitionSpecs(int isApproved) : base(x => x.Status.State == DocumentStatus.Unpaid && x.Status.State == DocumentStatus.Paid && x.Status.State == DocumentStatus.Partial) 
        {
            AddInclude(i => i.Status);
        }
        public RequisitionSpecs(int requestId , bool isReff) 
            : base (x => x.RequestId == requestId)
        {

        }
    }
}
