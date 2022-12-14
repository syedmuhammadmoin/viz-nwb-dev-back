using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Stock : BaseEntity<int>
    {
        public int ItemId { get; private set; }
        [ForeignKey("ItemId")]
        public Product Item { get; private set; }
        public int AvailableQuantity { get; private set; }
        public int ReservedQuantity { get; private set; }
        public int ReservedRequisitionQuantity { get; private set; }
        public int WarehouseId { get; private set; }
        [ForeignKey("WarehouseId")]
        public Warehouse Warehouse { get; private set; }

        public Stock(int itemId, int availableQuantity, int reservedQuantity, int warehouseId)
        {
            ItemId = itemId;
            AvailableQuantity = availableQuantity;
            ReservedQuantity = reservedQuantity;
            WarehouseId = warehouseId;
        }
        public void updateAvailableQuantity(int availableQuantity)
        {
            AvailableQuantity = availableQuantity;
        }
        public void updateReservedQuantity(int reservedQuantity)
        {
            ReservedQuantity = reservedQuantity;
        }
        public void updateRequisitionReservedQuantity(int reservedRequisitionQuantity)
        {
            ReservedRequisitionQuantity = reservedRequisitionQuantity;
        }

        protected Stock()
        {

        }
    }
}
