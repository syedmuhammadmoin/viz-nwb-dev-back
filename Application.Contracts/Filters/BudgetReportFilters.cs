using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Filters
{
    public class BudgetReportFilters
    {
        public DateTime To { get; set; }
        public string BudgetName { get; set; }
    }
}
