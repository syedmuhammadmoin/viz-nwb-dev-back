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
    public class IssuanceSpecs : BaseSpecification<IssuanceMaster>
    {
        public IssuanceSpecs(List<DateTime?> docDate,
            List<DocumentStatus?> states, TransactionFormFilter filter, bool isTotalRecord) : base(x => (docDate.Count() > 0 ? docDate.Contains(x.IssuanceDate) : true)
            && x.DocNo.Contains(filter.DocNo != null ? filter.DocNo : "")
            && x.Employee.Name.Contains(filter.BusinessPartner != null ? filter.BusinessPartner : "")
			&& (x.IssuanceDate >= (filter.StartDate != null ? filter.StartDate : x.IssuanceDate) && x.IssuanceDate <= (filter.EndDate != null ? filter.EndDate : x.IssuanceDate))		
			&& (states.Count() > 0 ? states.Contains(x.Status.State) : true))
        {
            if (!isTotalRecord)
            {
                var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
                ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
                ApplyOrderByDescending(i => i.Id);
                AddInclude(i => i.Employee);
                AddInclude(i => i.Status);
                AddInclude(i => i.Campus);
                AddInclude("IssuanceLines.Warehouse");
                AddInclude("IssuanceLines.Item");
            }
        }

        public IssuanceSpecs(bool forEdit)
        {
            if (forEdit)
            {
                AddInclude(i => i.IssuanceLines);
                AddInclude(i => i.Status);
            }
            else
            {
                AddInclude(i => i.Campus);
                AddInclude(i => i.Employee);
                AddInclude(i => i.Status);
                AddInclude(i => i.Requisition);
                AddInclude("IssuanceLines.Warehouse");
                AddInclude("IssuanceLines.Item");
            }
        }
        public IssuanceSpecs()
            : base(x => x.Status.State != DocumentStatus.Paid)
        {
            AddInclude(i => i.IssuanceLines);
        }

        public IssuanceSpecs(string workflow)
           : base(e => (e.Status.State != DocumentStatus.Unpaid && e.Status.State != DocumentStatus.Partial && e.Status.State != DocumentStatus.Paid && e.Status.State != DocumentStatus.Draft && e.Status.State != DocumentStatus.Cancelled))
        {
            AddInclude(i => i.Status);
        }

        public IssuanceSpecs(int requisitionId ,bool isRequisition) 
           : base(e => e.RequisitionId == requisitionId )
        {
            AddInclude(i => i.IssuanceLines);
        } 
    }
}
