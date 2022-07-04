using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class RequisitionToIssuanceLineReconcile : BaseEntity<int>
    {
        public int ItemId { get; private set; }
        [ForeignKey("ItemId")]
        public Product Item { get; private set; }
        public int Quantity { get; private set; }
        public int RequisitionId { get; private set; }
        [ForeignKey("RequisitionId")]
        public RequisitionMaster Requisition { get; private set; }
        public int IssuanceId { get; private set; }
        [ForeignKey("IssuanceId")]
        public IssuanceMaster Issuance { get; private set; }
        public int RequisitionLineId { get; private set; }
        [ForeignKey("RequisitionLineId")]
        public RequisitionLines RequisitionLines { get; private set; }
        public int IssuanceLineId { get; private set; }
        [ForeignKey("IssuanceLineId")]
        public IssuanceLines IssuanceLines { get; private set; }
        public int WarehouseId { get; private set; }
        [ForeignKey("WarehouseId")]
        public Warehouse Warehouse { get; private set; }

        public RequisitionToIssuanceLineReconcile(int itemId, int quantity, int requisitionId, int issuanceId, int requisitionLineId, int issuanceLineId, int warehouseId)
        {
            ItemId = itemId;
            Quantity = quantity;
            RequisitionId = requisitionId;
            IssuanceId = issuanceId;
            RequisitionLineId = requisitionLineId;
            IssuanceLineId = issuanceLineId;
            WarehouseId = warehouseId;
        }

        protected RequisitionToIssuanceLineReconcile()
        {

        }
    }
}
