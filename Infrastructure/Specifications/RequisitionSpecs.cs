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
        public RequisitionSpecs(PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
            ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
            AddInclude(i => i.Campus);
            AddInclude(i => i.Status);
            AddInclude(i => i.BusinessPartner);
            AddInclude("RequisitionLines.Item");
            AddInclude("RequisitionLines.Warehouse");
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
                AddInclude(i => i.BusinessPartner);
                AddInclude("RequisitionLines.Item");
                AddInclude("RequisitionLines.Warehouse");
            }
        }
        public RequisitionSpecs() : base(e => (e.Status.State != DocumentStatus.Unpaid && e.Status.State != DocumentStatus.Partial && e.Status.State != DocumentStatus.Paid && e.Status.State != DocumentStatus.Draft && e.Status.State != DocumentStatus.Cancelled))
        {
            AddInclude(i => i.Status);
        }
    }
}
