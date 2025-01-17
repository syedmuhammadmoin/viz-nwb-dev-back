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
    public class RequestSpecs : BaseSpecification<RequestMaster>
    {
        public RequestSpecs(List<DateTime?> docDate,
            List<DocumentStatus?> states, TransactionFormFilter filter, bool isTotalRecord)
            : base(x => (docDate.Count() > 0 ? docDate.Contains(x.RequestDate) : true)
               && x.DocNo.Contains(filter.DocNo != null ? filter.DocNo : "")
            && x.Employee.Name.Contains(filter.BusinessPartner != null ? filter.BusinessPartner : "")
			&& (x.RequestDate >= (filter.StartDate != null ? filter.StartDate : x.RequestDate) && x.RequestDate <= (filter.EndDate != null ? filter.EndDate : x.RequestDate))	
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
            }
        }
        public RequestSpecs(bool forEdit)
        {
            if (forEdit)
            {
                AddInclude(i => i.RequestLines);
                AddInclude(i => i.Status);
            }
            else
            {
                AddInclude(i => i.Campus);
                AddInclude(i => i.Status);
                AddInclude(i => i.Employee);
                AddInclude(i => i.RequestLines);
            }
        }

        public RequestSpecs() : base(x => x.Status.State != DocumentStatus.Paid)
        {
            AddInclude(i => i.RequestLines);
        }

        public RequestSpecs(string workflow)
         : base(e => (e.Status.State != DocumentStatus.Unpaid && e.Status.State != DocumentStatus.Partial && e.Status.State != DocumentStatus.Paid && e.Status.State != DocumentStatus.Draft && e.Status.State != DocumentStatus.Cancelled))
        {
            AddInclude(i => i.Status);
        }
    }
}
