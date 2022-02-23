using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class InvoiceLinesDto
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Tax { get; set; }
        public decimal SubTotal { get; set; }
        public Guid AccountId { get; set; }
        public string AccountName { get; set; }
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public int MasterId { get; set; }
    }
}
