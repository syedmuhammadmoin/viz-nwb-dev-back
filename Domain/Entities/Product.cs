using Domain.Base;
using Domain.Constants;
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
        public PurchasedOrSold PurchasedOrSold { get; private set; }
        public ProductType ProductType { get; private set; }
        public int CategoryId { get; private set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal SalesPrice { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Cost { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal SalesTax { get; private set; }
        [MaxLength(100)]
        public string Barcode { get; private set; }

        protected Product()
        {

        }
    }
}
