using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreateRequisitionLinesDto
    {
        public int Id { get; set; }
        [Required]
        public int ItemId { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }
        [Required]
        [StringLength(200)]
        public string Description { get; set; }
        public int? WarehouseId { get; set; }
    }
}
