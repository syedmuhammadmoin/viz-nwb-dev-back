﻿using Application.Contracts.Filters;
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
            AddInclude("InvoiceLines.Account");
            AddInclude("InvoiceLines.Location");
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
                AddInclude("InvoiceLines.Account");
                AddInclude("InvoiceLines.Location");
                AddInclude("InvoiceLines.Item");
            }

        }
    }
}