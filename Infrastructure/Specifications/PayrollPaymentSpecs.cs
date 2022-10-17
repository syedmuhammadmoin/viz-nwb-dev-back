using Domain.Constants;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class PayrollPaymentSpecs : BaseSpecification<Payment>
    {
        public PayrollPaymentSpecs() : base(x => x.Status.State == DocumentStatus.Draft || x.Status.State == DocumentStatus.Rejected)
        {
            AddInclude(i => i.Status);
            AddInclude(i => i.BusinessPartner);
            ApplyAsNoTracking();
        }
    }
}
