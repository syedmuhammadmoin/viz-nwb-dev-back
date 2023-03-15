using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Constants;

namespace Application.Contracts.DTOs
{
    public class CreateFixedAssetLinesDto
    {
        [Required]
        public int FixedAssetId { get; set; }
        [Required]
        public int ActiveDays { get; set; }
        [Required]
        public DateTime ActiveDate { get; set; }
        public DateTime? InactiveDate { get; set; }
    }
}
