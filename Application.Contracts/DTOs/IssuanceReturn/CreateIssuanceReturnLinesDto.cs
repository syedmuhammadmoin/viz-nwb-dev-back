using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreateIssuanceReturnLinesDto
    {
        public int Id { get; set; }
        [Required]
        public int? ItemId { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int? Quantity { get; set; }
        [Required]
        public int? WarehouseId { get; set; }
        public int? FixedAssetId { get;  set; }
       
    }
}
