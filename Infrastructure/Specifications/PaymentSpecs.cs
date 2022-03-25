using Application.Contracts.Filters;
using Domain.Constants;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class PaymentSpecs : BaseSpecification<Payment>
    {
        public PaymentSpecs(PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
            ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
            AddInclude(i => i.BusinessPartner);
            AddInclude(i => i.Account);
            AddInclude(i => i.Campus);
            AddInclude(i => i.PaymentRegister);
        }

        public PaymentSpecs()
        {
            AddInclude(i => i.BusinessPartner);
            AddInclude(i => i.Account);
            AddInclude(i => i.Campus);
            AddInclude(i => i.PaymentRegister);
        }

        public PaymentSpecs(bool forRecon) : base(p => p.BankReconStatus != DocumentStatus.Reconciled)
        {
        }
        public PaymentSpecs(int transactionId) :
            base(p => (p.Status.State == DocumentStatus.Unpaid
            || p.Status.State == DocumentStatus.Partial) && (p.TransactionId == transactionId))
        { 
        
        }
    }
}
