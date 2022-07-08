using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class IssuanceToGRNLineReconcile : BaseEntity<int>
    {
        public int ItemId { get; private set; }
        [ForeignKey("ItemId")]
        public Product Item { get; private set; }
        public int Quantity { get; private set; }
        public int IssuanceId { get; private set; }
        [ForeignKey("IssuanceId")]
        public IssuanceMaster Issuance { get; private set; }
        public int GRNId { get; private set; }
        [ForeignKey("GRNId")]
        public GRNMaster GRN { get; private set; }
        public int IssuanceLineId { get; private set; }
        [ForeignKey("IssuanceLineId")]
        public IssuanceLines IssuanceLines { get; private set; }
        public int GRNLineId { get; private set; }
        [ForeignKey("GRNLineId")]
        public GRNLines GRNLines { get; private set; }
        public int WarehouseId { get; private set; }
        [ForeignKey("WarehouseId")]
        public Warehouse Warehouse { get; private set; }

        public IssuanceToGRNLineReconcile(int itemId, int quantity, int issuanceId, int grnId, int issuanceLineId, int grnLineId,int warehouse)
        {
            ItemId = itemId;
            Quantity = quantity;
            GRNId = grnId;
            IssuanceId = issuanceId;
            IssuanceLineId = issuanceLineId;
            GRNLineId = grnLineId;
            WarehouseId = warehouse;
        }
        protected IssuanceToGRNLineReconcile()
        {

        }
    }
}
