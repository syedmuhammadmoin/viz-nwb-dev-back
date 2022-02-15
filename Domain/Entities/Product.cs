using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Product : BaseEntity<int>
    {
        [MaxLength(100)]
        public string ProductName { get; private set; }
        public int PurchasedOrSold { get; private set; }
        public int ProductType { get; private set; }
        public int ProductCategoryId { get; private set; }
        [ForeignKey("ProductCategoryId")]
        public Category Category { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal SalesPrice { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Cost { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal SalesTax { get; private set; }
        [MaxLength(50)]
        public string Barcode { get; private set; }

        public Product(Product product)
        {
            ProductName = product.ProductName;
            PurchasedOrSold = product.PurchasedOrSold;
            ProductType = product.ProductType;
            ProductCategoryId = product.ProductCategoryId;
            Category = product.Category;
            SalesPrice = product.SalesPrice;
            Cost = product.Cost;
            SalesTax = product.SalesTax;
            Barcode = product.Barcode;
        }
        protected Product()
        {

        }
    }
}
