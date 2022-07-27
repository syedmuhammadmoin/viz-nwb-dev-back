using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class POToGRNLineReconcileSpecs : BaseSpecification<POToGRNLineReconcile>
    {
        public POToGRNLineReconcileSpecs(int purchaseOrderId, int purchaseOrderLineId, int itemId, int warehouseId) 
            : base(x => x.PurchaseOrderId == purchaseOrderId
            && x.PurchaseOrderLineId == purchaseOrderLineId
            && x.ItemId == itemId
            && x.WarehouseId == warehouseId)
        {
            ApplyAsNoTracking();
        }

        public POToGRNLineReconcileSpecs(bool isPO,int id ) 
            : base(x => isPO ? x.PurchaseOrderId == id : x.GRNId == id)
        {
            AddInclude(x => x.GRN);
        }
    }
}
