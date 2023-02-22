using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreateDepreciationAdjustmentDto
    {
        public int? Id { get; set; }
        [Required]
        public DateTime DateOfDepreciationAdjustment { get;  set; }
        [Required]
        public int? CampusId { get;  set; }
        [MaxLength(500)]
        public string Description { get; set; }
        [Required]
        public bool IsSubmit { get; set; }
        [Required]
        public virtual List<CreateDepreciationAdjustmentLinesDto> DepreciationAdjustmentLines { get; set; }
    }
}
