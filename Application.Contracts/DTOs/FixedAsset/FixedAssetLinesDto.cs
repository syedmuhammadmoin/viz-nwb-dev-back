using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Constants;

namespace Application.Contracts.DTOs
{
    public class FixedAssetLinesDto
    {
        public int MasterId { get; set; }
        public int ActiveDays { get; set; }
        public DateTime ActiveDate { get; set; }
        public DateTime? InactiveDate { get; set; }
    }
}
