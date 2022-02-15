using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreateLocationDto
    {
        public int? Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public string Dimensions { get; set; }
        [Required]
        [StringLength(100)]
        public string Supervisor { get; set; }
        [Required]
        public int WarehouseId { get; set; }
    }
}
