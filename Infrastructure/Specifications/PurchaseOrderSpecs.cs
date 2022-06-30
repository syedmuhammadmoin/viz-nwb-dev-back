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
    public class PurchaseOrderSpecs : BaseSpecification<PurchaseOrderMaster>
    {
        public PurchaseOrderSpecs(PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
            ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
            ApplyOrderByDescending(i => i.Id);
            AddInclude(i => i.Campus);
            AddInclude(i => i.Status);
            AddInclude(i => i.Vendor);
            AddInclude("PurchaseOrderLines.Account");
            AddInclude("PurchaseOrderLines.Item");
            AddInclude("PurchaseOrderLines.Warehouse");
        }

        public PurchaseOrderSpecs(bool forEdit)
        {
            if (forEdit)
            {
                AddInclude(i => i.PurchaseOrderLines);
                AddInclude(i => i.Status);
            }
            else
            {
                AddInclude(i => i.Campus);
                AddInclude(i => i.Status);
                AddInclude(i => i.Vendor);
                AddInclude("PurchaseOrderLines.Account");
                AddInclude("PurchaseOrderLines.Item");
                AddInclude("PurchaseOrderLines.Warehouse");
            }
        }
        public PurchaseOrderSpecs()
            : base(x => x.Status.State != DocumentStatus.Paid)
        {
            AddInclude(i => i.PurchaseOrderLines);
        }
        public PurchaseOrderSpecs(int id) : base(x => x.Id == id)
        {
            AddInclude(i => i.PurchaseOrderLines);
        }
    }
}
