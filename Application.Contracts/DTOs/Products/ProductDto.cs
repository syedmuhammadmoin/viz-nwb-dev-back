using Domain.Constants;
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
        public ProductType ProductType { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal SalesPrice { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SalesTax { get; set; }
        public string Barcode { get; set; }
    }
}
