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
        public ProductType ProductType { get; private set; }
        public int CategoryId { get; private set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; private set; }
        public int UnitOfMeasurementId { get; private set; }
        [ForeignKey("UnitOfMeasurementId")]
        public UnitOfMeasurement UnitOfMeasurement { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal SalesPrice { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal PurchasePrice { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal SalesTax { get; private set; }
        [MaxLength(100)]
        public string Barcode { get; private set; }

        protected Product()
        {

        }
    }
}
