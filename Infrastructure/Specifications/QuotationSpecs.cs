
using Application.Contracts.Filters;
using Domain.Constants;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class QuotationSpecs : BaseSpecification<QuotationMaster>
    {
        public QuotationSpecs(List<DateTime?> docDate,
          List<DocumentStatus?> states, TransactionFormFilter filter, bool isTotalRecord)
          : base(x => (docDate.Count() > 0 ? docDate.Contains(x.QuotationDate) : true)
             && x.DocNo.Contains(filter.DocNo != null ? filter.DocNo : "")
          && x.Vendor.Name.Contains(filter.BusinessPartner != null ? filter.BusinessPartner : "")
		   && x.QuotationDate.Month == (filter.Month != null ? Convert.ToInt32(filter.Month) : x.QuotationDate.Month)
			&& x.QuotationDate.Year == (filter.Year != null ? Convert.ToInt32(filter.Year) : x.QuotationDate.Year)
		  && (states.Count() > 0 ? states.Contains(x.Status.State) : true))
        {
            if (!isTotalRecord)
            {
                var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
                ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
                ApplyOrderByDescending(i => i.Id);
                AddInclude(i => i.Status);
                AddInclude(i => i.Vendor);
                AddInclude("QuotationLines.Item");
            }
        }
        
        public QuotationSpecs(bool forEdit)
        {
            if (forEdit)
            {
                AddInclude(i => i.QuotationLines);
                AddInclude(i => i.Status);
            }
            else
            {
                AddInclude(i => i.Status);
                AddInclude(i => i.Vendor);
                AddInclude(i => i.QuotationLines);
                AddInclude("QuotationLines.Item");
            }
        }
        
        public QuotationSpecs(bool abc, int quoatationCompId) : base(x => x.QuotationComparativeId == (int)quoatationCompId)
        {
            AddInclude(i => i.QuotationLines);
            AddInclude(i => i.Vendor);
            AddInclude(i => i.Status);
            AddInclude("QuotationLines.Item");
        }

        public QuotationSpecs(string workflow)
         : base(e => (e.Status.State != DocumentStatus.Unpaid && e.Status.State != DocumentStatus.Partial && e.Status.State != DocumentStatus.Paid && e.Status.State != DocumentStatus.Draft && e.Status.State != DocumentStatus.Cancelled))
        {
            AddInclude(i => i.Status);
        }

        public QuotationSpecs(int requisitionId, int? quotationCompId)
            : base(x => x.RequisitionId == requisitionId 
            && x.Status.State == DocumentStatus.Unpaid 
            && (x.QuotationComparativeId == null
            || x.QuotationComparativeId == quotationCompId))
        {
            ApplyOrderByDescending(i => i.Id);
            AddInclude(i => i.QuotationLines);
            AddInclude(i => i.Vendor);
            AddInclude(i => i.Status);
            AddInclude("QuotationLines.Item");
        }
        public QuotationSpecs(int requisitionId ,bool isReff)
           : base(x => x.RequisitionId == requisitionId )
        {
            AddInclude(i => i.QuotationLines);
            AddInclude("QuotationLines.Item");
        }
    }
}
