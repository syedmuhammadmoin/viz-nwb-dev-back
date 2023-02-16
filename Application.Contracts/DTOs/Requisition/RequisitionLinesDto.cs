using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs

{
    public class RequisitionLinesDto
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string Item { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public int ReserveQuantity { get;  set; }
        public decimal PurchasePrice { get; set; }
        public decimal SubTotal { get; set; }
        public int IssuedQuantity { get; set; }
        public int PendingQuantity { get; set; }
        public int AvailableQuantity { get; set; }
        public int? WarehouseId { get; set; }
        public string Warehouse { get; set; }
        public int MasterId { get; set; }
        public int? FixedAssetId { get; private set; }

    }
}
