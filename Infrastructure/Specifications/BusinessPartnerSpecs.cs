﻿using Application.Contracts.Filters;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class BusinessPartnerSpecs : BaseSpecification<BusinessPartner>
    {
        public BusinessPartnerSpecs(PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
            ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
            AddInclude(i=> i.AccountPayable);
            AddInclude(i=> i.AccountReceivable);
        }

        public BusinessPartnerSpecs()
        {
            AddInclude(i => i.AccountPayable);
            AddInclude(i => i.AccountReceivable);
        }
    }
}