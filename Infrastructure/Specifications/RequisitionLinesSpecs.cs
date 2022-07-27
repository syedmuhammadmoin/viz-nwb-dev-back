using Domain.Constants;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class RequisitionLinesSpecs : BaseSpecification<RequisitionLines>
    {
        public RequisitionLinesSpecs(int itemId, int masterId)
        : base(x => x.ItemId == itemId && x.MasterId == masterId
        && (x.Status == DocumentStatus.Partial || x.Status == DocumentStatus.Unreconciled))
        {
        }
    }
}
