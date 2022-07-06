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
        public GRNToGoodsReturnNoteReconcileSpecs(int grnId, int grnLineId, int itemId, int warehouseId)
            : base(x => x.GRNId == grnId
            && x.GRNLineId == grnLineId
            && x.ItemId == itemId
            && x.WarehouseId == warehouseId)
        {
            ApplyAsNoTracking();
        }

        public GRNToGoodsReturnNoteReconcileSpecs(int id)
            : base(x => x.GRNId == id)
        {
            AddInclude(x => x.GoodsReturnNote);
        }
    }
}
