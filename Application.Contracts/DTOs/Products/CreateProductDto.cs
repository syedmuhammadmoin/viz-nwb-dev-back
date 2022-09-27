using Domain.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreateProductDto
    {
        public int? Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string ProductName { get; set; }
        [Required]
        public ProductType ProductType { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int UnitOfMeasurementId { get; set; }
        [Required]
        public decimal? SalesPrice { get; set; }
        [Required]
        public decimal? PurchasePrice { get; set; }
        [Required]
        [Range(0.00, 100.00, ErrorMessage = "Please enter a value between 1 and 100")] 
        public decimal? SalesTax { get; set; }
        [MaxLength(100)]
        public string Barcode { get; set; }
    }
}
