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
    public class TaxesSpecs : BaseSpecification<Taxes>
    {
        public TaxesSpecs(TransactionFormFilter filter, bool isTotalRecord)
            : base(c => c.Name.Contains(filter.Name != null ? filter.Name : ""))
        {
            if (!isTotalRecord)
            {
                var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
                ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
                ApplyOrderByDescending(i => i.Id);
                AddInclude(i => i.Account);
            }
        }
        public TaxesSpecs()
        {
            AddInclude(i => i.Account);
            AddInclude(i => i.TaxInvoicesLines);
            AddInclude(i => i.TaxRefundLines);
            AddInclude(i => i.ChildrenTaxes);
            AddInclude("TaxInvoicesLines.Account");
            AddInclude("TaxRefundLines.Account");

        }
        public TaxesSpecs(TaxType taxType) : base(x => x.TaxType == taxType)
        {
            
        }
    }
}
