using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Filters
{
    public class TrialBalanceFilters
    {
        [Required]
        public DateTime? DocDate { get; set; }
        [Required]
        public DateTime? DocDate2 { get; set; }
        public string? AccountId { get; set; }
        public int? CampusId { get; set; }
    }
}
