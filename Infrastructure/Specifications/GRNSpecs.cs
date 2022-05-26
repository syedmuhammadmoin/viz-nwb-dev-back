﻿using Application.Contracts.Filters;
using Domain.Constants;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class GRNSpecs : BaseSpecification<GRNMaster>
    {
        public GRNSpecs(PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
            ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
            ApplyOrderByDescending(i => i.Id);
            AddInclude(i => i.Campus);
            AddInclude(i => i.Status);
            AddInclude(i => i.Vendor);
            AddInclude(i => i.PurchaseOrder);
            AddInclude("GRNLines.Item");
            AddInclude("GRNLines.Warehouse");
        }

        public GRNSpecs(bool forEdit)
        {
            if (forEdit)
            {
                AddInclude(i => i.GRNLines);
                AddInclude(i => i.Status);
            }
            else
            {
                AddInclude(i => i.Campus);
                AddInclude(i => i.Status);
                AddInclude(i => i.Vendor);
                AddInclude(i => i.PurchaseOrder);
                AddInclude("GRNLines.Item");
                AddInclude("GRNLines.Warehouse");
            }
        }
        public GRNSpecs() : base(e => (e.Status.State != DocumentStatus.Unpaid && e.Status.State != DocumentStatus.Partial && e.Status.State != DocumentStatus.Paid && e.Status.State != DocumentStatus.Draft && e.Status.State != DocumentStatus.Cancelled))
        {
            AddInclude(i => i.Status);
        }
    }
}
