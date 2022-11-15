using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Filters
{
    public class PayrollExecutiveReportFilter
    {
        public int? CampusId { get; set; }
        public int? PayrollItemId { get; set; }
        public int?[] Month { get; set; }
        [Required]
        public int? Year { get; set; }
    }
}
