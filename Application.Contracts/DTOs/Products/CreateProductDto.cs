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
        public string ProductName { get; set; }
        [Required]
        public int PurchasedOrSold { get; set; }
        [Required]
        public int ProductType { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public decimal SalesPrice { get; set; }
        [Required]
        public decimal Cost { get; set; }
        [Required]
        public decimal SalesTax { get; set; }
        public string Barcode { get; set; }
    }
}
