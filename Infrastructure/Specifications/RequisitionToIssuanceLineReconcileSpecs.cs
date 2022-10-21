using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class RequisitionToIssuanceLineReconcileSpecs : BaseSpecification<RequisitionToIssuanceLineReconcile>
    {
        public RequisitionToIssuanceLineReconcileSpecs(int requisitionId, int requisitionLineId, int itemId)
            : base(x => x.RequisitionId == requisitionId
            && x.RequisitionLineId == requisitionLineId
            && x.ItemId == itemId
            )
        {
        }

        public RequisitionToIssuanceLineReconcileSpecs(bool isReq, int id)
            : base(x => isReq ? x.RequisitionId == id : x.IssuanceId == id)
        {
            AddInclude(x => x.Issuance);
        }
    }
}
