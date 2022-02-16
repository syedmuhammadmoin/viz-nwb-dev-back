using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int PurchasedOrSold { get; set; }
        public int ProductType { get; set; }
        public int ProductCategoryId { get; set; }
        public string Category { get; set; }
        public decimal SalesPrice { get; set; }
        public decimal Cost { get; set; }
        public decimal SalesTax { get; set; }
        public string Barcode { get; set; }
        public string CategoryName { get; set; }
    }
}
