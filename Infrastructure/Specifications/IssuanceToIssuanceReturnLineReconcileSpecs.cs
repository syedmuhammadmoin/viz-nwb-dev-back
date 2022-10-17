using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class IssuanceToIssuanceReturnLineReconcileSpecs : BaseSpecification<IssuanceToIssuanceReturnLineReconcile>
    {
        public IssuanceToIssuanceReturnLineReconcileSpecs(int issuanceId, int issuanceLineId, int itemId, int warehouseId)
           : base(x => x.IssuanceId == issuanceId
           && x.IssuanceLineId == issuanceLineId
           && x.ItemId == itemId
           && x.WarehouseId == warehouseId)
        {
            ApplyAsNoTracking();
        }

        public IssuanceToIssuanceReturnLineReconcileSpecs(bool isIssuance, int id)
            : base(x => isIssuance ? x.IssuanceId == id : x.IssuanceReturnId == id)
        {
            AddInclude(x => x.IssuanceReturn);
            ApplyAsNoTracking();
        }
    }
}
