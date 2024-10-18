using Application.Contracts.Filters;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class AccountingSettingSpecs : BaseSpecification<AccountingSettingEntity>
    {
        public AccountingSettingSpecs(TransactionFormFilter filter, bool isTotalRecord)
        {
            if (!isTotalRecord)
            {
                var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
                ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
                ApplyOrderByDescending(i => i.Id);
            }
        }
        public AccountingSettingSpecs(bool value)
        {
            AddInclude(x => x.BaseTaxReceivedAccount);
            AddInclude(x => x.Country);
            AddInclude(x => x.Currency);
        }
    }
}
