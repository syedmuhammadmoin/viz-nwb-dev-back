using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class GRNToGoodsReturnNoteLineReconcileSpecs : BaseSpecification<GRNToGoodsReturnNoteLineReconcile>
    {
        public GRNToGoodsReturnNoteLineReconcileSpecs(int grnId, int grnLineId, int itemId, int warehouseId)
            : base(x => x.GRNId == grnId
            && x.GRNLineId == grnLineId
            && x.ItemId == itemId
            && x.WarehouseId == warehouseId)
        {
            ApplyAsNoTracking();
        }

        public GRNToGoodsReturnNoteLineReconcileSpecs(int id)
            : base(x => x.GRNId == id)
        {
            AddInclude(x => x.GoodsReturnNote);
            ApplyAsNoTracking();
        }
    }
}
