    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class PurchaseOrderLinesDto
    {
        public int Id { get; private set; }
        public int ItemId { get; private set; }
        public string Item { get; private set; }
        public string Description { get; private set; }
        public int Quantity { get; private set; }
        public decimal Cost { get; private set; }
        public decimal Tax { get; private set; }
        public Guid AccountId { get; private set; }
        public string AccountName { get; private set; }
        public int? WarehouseId { get; private set; }
        public string Warehouse { get; private set; }
        public int MasterId { get; private set; }
    }
}
