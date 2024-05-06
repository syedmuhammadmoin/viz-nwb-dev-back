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
    public class PettyCashSpecs : BaseSpecification<PettyCashMaster>
    {
        public PettyCashSpecs(List<DateTime?> docDate, List<DateTime?> dueDate,
            List<DocumentStatus?> states, TransactionFormFilter filter, bool isTotalRecord)
            : base(c => (docDate.Count() > 0 ? docDate.Contains(c.Date) : true)
                && c.DocNo.Contains(filter.DocNo != null ? filter.DocNo : "")
                && (states.Count() > 0 ? states.Contains(c.Status.State) : true))
        {
            if (!isTotalRecord)
            {
                var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
                ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
                AddInclude(i => i.Campus);
                AddInclude(i => i.Status);
                ApplyOrderByDescending(i => i.Id);
                AddInclude("PettyCashLines.BusinessPartner");
                AddInclude("PettyCashLines.Account");
                
            }
        }

        public PettyCashSpecs(bool forEdit)
        {
            if (forEdit)
            {
                AddInclude(i => i.PettyCashLines);
                AddInclude(i => i.Status);
            }
            else
            {
                AddInclude(i => i.Campus);
                AddInclude(i => i.Status);
                AddInclude("PettyCashLines.BusinessPartner");
                AddInclude("PettyCashLines.Account");
                
            }
        }
        public PettyCashSpecs() : base(e => (e.Status.State != DocumentStatus.Unpaid && e.Status.State != DocumentStatus.Partial && e.Status.State != DocumentStatus.Paid && e.Status.State != DocumentStatus.Draft && e.Status.State != DocumentStatus.Cancelled))
        {
            AddInclude(i => i.Status);
        }
    }
}