using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class POToGRNLineReconcile : BaseEntity<int>
    {
        public int ItemId { get; private set; }
        [ForeignKey("ItemId")]
        public Product Item { get; private set; }
        public int Quantity { get; private set; }
        public int PurchaseOrderId { get; private set; }
        [ForeignKey("PurchaseOrderId")]
        public PurchaseOrderMaster PurchaseOrder { get; private set; }
        public int GRNId { get; private set; }
        [ForeignKey("GRNId")]
        public GRNMaster GRN { get; private set; }
        public int PurchaseOrderLineId { get; private set; }
        [ForeignKey("PurchaseOrderLineId")]
        public PurchaseOrderLines PurchaseOrderLines { get; private set; }
        public int GRNLineId { get; private set; }
        [ForeignKey("GRNLineId")]
        public GRNLines GRNLines { get; private set; }
        public int WarehouseId { get; private set; }
        [ForeignKey("WarehouseId")]
        public Warehouse Warehouse { get; private set; }

        public POToGRNLineReconcile(int itemId, int quantity, int purchaseOrderId, int gRNId, int purchaseOrderLineId, int gRNLineId, int warehouse)
        {
            ItemId = itemId;
            Quantity = quantity;
            PurchaseOrderId = purchaseOrderId;
            GRNId = gRNId;
            PurchaseOrderLineId = purchaseOrderLineId;
            GRNLineId = gRNLineId;
            WarehouseId = warehouse;
        }
        protected POToGRNLineReconcile()
        {

        }
    }
}
