using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class BillLinesDto
    {
        public int Id { get; set; }
        public int? ItemId { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal Cost { get; set; }
        public decimal Tax { get; set; }
        public decimal AnyOtherTax { get; set; }
        public decimal SubTotal { get; set; }
        public string AccountId { get; set; }
        public string AccountName { get; set; }
        public int? WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public int MasterId { get; set; }
    }
}
