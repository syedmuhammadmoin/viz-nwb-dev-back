using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class GRNToGoodsReturnNoteLineReconcile : BaseEntity<int>
    {
        public int ItemId { get; private set; }
        [ForeignKey("ItemId")]
        public Product Item { get; private set; }
        public int Quantity { get; private set; }
        public int GoodsReturnNoteId { get; private set; }
        [ForeignKey("GoodsReturnNoteId")]
        public GoodsReturnNoteMaster GoodsReturnNote { get; private set; }
        public int GRNId { get; private set; }
        [ForeignKey("GRNId")]
        public GRNMaster GRN { get; private set; }
        public int GoodsReturnNoteLineId { get; private set; }
        [ForeignKey("GoodsReturnNoteLineId")]
        public GoodsReturnNoteLines GoodsReturnNoteLines { get; private set; }
        public int GRNLineId { get; private set; }
        [ForeignKey("GRNLineId")]
        public GRNLines GRNLines { get; private set; }
        public int WarehouseId { get; private set; }
        [ForeignKey("WarehouseId")]
        public Warehouse Warehouse { get; private set; }

        public GRNToGoodsReturnNoteLineReconcile(int itemId, int quantity, int goodsReturnNoteId, int gRNId, int goodsReturnNoteLineId, int gRNLineId, int warehouse)
        {
            ItemId = itemId;
            Quantity = quantity;
            GoodsReturnNoteId = goodsReturnNoteId;
            GRNId = gRNId;
            GoodsReturnNoteLineId = goodsReturnNoteLineId;
            GRNLineId = gRNLineId;
            WarehouseId = warehouse;
        }
        protected GRNToGoodsReturnNoteLineReconcile()
        {

        }
    }
}
