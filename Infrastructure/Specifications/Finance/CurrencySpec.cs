using Application.Contracts.Filters;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class CurrencySpec : BaseSpecification<Currency>
    {
        public CurrencySpec(CurrencyFilter filter, bool isTotalRecord) : base(c =>
        c.Code.Contains(filter.Code != null ? filter.Code : "")
        && c.Name.Contains(filter.Name != null ? filter.Name : "")
        && c.Symbol.Contains(filter.Symbol != null ? filter.Symbol : "")
        //&& c.LastUpdate.Equals(filter.LastUpdate != null ? filter.LastUpdate : null)
        //&& c.UnitPerUSD.Equals(filter.UnitPerUSD != null ? filter.UnitPerUSD : "")
        )
        {

            if (!isTotalRecord)
            {
                var pageFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
                ApplyPaging(pageFilter.PageStart, pageFilter.PageEnd - pageFilter.PageStart);
                ApplyOrderByDescending(i=>i.ModifiedDate);
            }
        }
        public CurrencySpec() {

            AddInclude(i => i.CurrencyLines);
        }

    }
}
