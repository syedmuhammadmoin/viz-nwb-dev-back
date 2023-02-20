using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreateGRNLinesDto
    {
        public int Id { get; set; }
        [Required]
        public int? ItemId { get; set; }
        [StringLength(200)]
        public string Description { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int? Quantity { get; set; }
        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal? Cost { get; set; }
        [Range(0.00, 100.00, ErrorMessage = "Please enter a positive value")]
        public decimal Tax { get; set; }
        [Required]
        public int? WarehouseId { get; set; }
        public bool IsFixedAssetCreated { get;  set; }



    }
}
