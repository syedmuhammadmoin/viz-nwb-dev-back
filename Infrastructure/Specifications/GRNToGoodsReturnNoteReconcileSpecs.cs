using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class GRNToGoodsReturnNoteReconcileSpecs : BaseSpecification<GRNToGoodsReturnNoteReconcile>
    {
        public GRNToGoodsReturnNoteReconcileSpecs(int purchaseOrderId, int purchaseOrderLineId, int itemId, int warehouseId)
            : base(x => x.GRNId == purchaseOrderId
            && x.GRNLineId == purchaseOrderLineId
            && x.ItemId == itemId
            && x.WarehouseId == warehouseId)
        {
            ApplyAsNoTracking();
        }

        public GRNToGoodsReturnNoteReconcileSpecs(bool isPO, int id)
            : base(x => isPO ? x.GRNId == id : x.GRNId == id)
        {
            AddInclude(x => x.GRN);
        }
    }
}
