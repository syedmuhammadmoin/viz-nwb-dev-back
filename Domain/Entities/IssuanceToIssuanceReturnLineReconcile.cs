using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class IssuanceToIssuanceReturnLineReconcile : BaseEntity<int>
    {
        public int ItemId { get; private set; }
        [ForeignKey("ItemId")]
        public Product Item { get; private set; }
        public int Quantity { get; private set; }
        public int IssuanceId { get; private set; }
        [ForeignKey("IssuanceId")]
        public IssuanceMaster Issuance { get; private set; }
        public int IssuanceReturnId { get; private set; }
        [ForeignKey("IssuanceReturnId")]
        public IssuanceReturnMaster IssuanceReturn { get; private set; }
        public int IssuanceLineId { get; private set; }
        [ForeignKey("IssuanceLineId")]
        public IssuanceLines IssuanceLines { get; private set; }
        public int IssuanceReturnLineId { get; private set; }
        [ForeignKey("IssuanceReturnLineId")]
        public IssuanceReturnLines IssuanceReturnLines { get; private set; }
        public int WarehouseId { get; private set; }
        [ForeignKey("WarehouseId")]
        public Warehouse Warehouse { get; private set; }

        public IssuanceToIssuanceReturnLineReconcile(int itemId, int quantity, int issuanceId, int issuanceReturnId, int issuanceLineId, int issuanceReturnLineId,int warehouse)
        {
            ItemId = itemId;
            Quantity = quantity;
            IssuanceReturnId = issuanceReturnId;
            IssuanceId = issuanceId;
            IssuanceLineId = issuanceLineId;
            IssuanceReturnLineId = issuanceReturnLineId;
            WarehouseId = warehouse;
        }
        protected IssuanceToIssuanceReturnLineReconcile()
        {

        }
    }
}
