using Application.Contracts.Filters;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class BankAccountSpecs : BaseSpecification<BankAccount>
    {
        public BankAccountSpecs(TransactionFormFilter filter) 
            : base(c => (c.BankName.Contains(filter.BusinessPartner != null ? filter.BusinessPartner : "")
         && c.AccountTitle.Contains(filter.DocNo != null ? filter.DocNo : "")))
        {
            var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
            ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
            ApplyOrderByDescending(i => i.Id);
            AddInclude(i => i.Campus);
            AddInclude(i => i.ChAccount);
            AddInclude(i => i.ClearingAccount);

        }

        public BankAccountSpecs()
        {
            AddInclude(i => i.Campus);
            AddInclude(i => i.ChAccount);
            AddInclude(i => i.ClearingAccount);
        }
        public BankAccountSpecs(Guid clearingAccountId) : base(e => e.ClearingAccountId == clearingAccountId)
        {
        }
    }
}
