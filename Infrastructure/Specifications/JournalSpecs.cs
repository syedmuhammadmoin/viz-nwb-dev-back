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
    public class JournalSpecs : BaseSpecification<Journal>
    {
        public JournalSpecs(TransactionFormFilter filter, bool isTotalRecord)
          : base(c => c.Name.Contains(filter.Name != null ? filter.Name : ""))
        {
            if (!isTotalRecord)
            {
                var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
                ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
                ApplyOrderByDescending(i => i.Id);
             
             
            }
            AddInclude(i => i.DefaultAccount);
            AddInclude(i => i.ProfitAccount);
            AddInclude(i => i.LossAccount);
            AddInclude(i => i.SuspenseAccount);
        }

        public JournalSpecs()
        {
          
        }

    }
}
