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
        public string Campus { get; set; }
        public string PayrollItem { get; set; }
        public int?[] Month { get; set; }
        [Required]
        public int? Year { get; set; }
    }
}
