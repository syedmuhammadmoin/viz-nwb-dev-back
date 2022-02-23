﻿using Application.Contracts.Filters;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class JournalEntrySpecs : BaseSpecification<JournalEntryMaster>
    {
        public JournalEntrySpecs(PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
            ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
            AddInclude("JournalEntryLines.BusinessPartner");
            AddInclude("JournalEntryLines.Account");
            AddInclude("JournalEntryLines.Location");
        }

        public JournalEntrySpecs()
        {
            AddInclude("JournalEntryLines.BusinessPartner");
            AddInclude("JournalEntryLines.Account");
            AddInclude("JournalEntryLines.Location");
        }
    }
}