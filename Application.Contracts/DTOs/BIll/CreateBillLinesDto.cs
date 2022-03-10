using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreateBillLinesDto
    {
        public int Id { get; set; }
        [Required]
        public Guid AccountId { get; set; }
        public int? ItemId { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }
        [Required]
        [Range(1, float.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Cost { get; set; }
        [Range(0.00, 100.00, ErrorMessage = "Please enter a positive value")]
        public decimal Tax { get; set; }
        public int? LocationId { get; set; }
    }
}
