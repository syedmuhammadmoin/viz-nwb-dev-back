using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Filters
{
    public class BalanceSheetFilters
    {
        [Required]
        public DateTime DocDate { get; set; }
        public int? CampusId { get; set; }
    }
}
