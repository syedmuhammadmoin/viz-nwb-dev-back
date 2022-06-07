using Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Filters
{
    public class PayrollItemFilter : PaginationFilter
    {
        public string Name { get; set; }
        public CalculationType? PayrollItemType { get; set; }
        public PayrollType? PayrollType { get; set; }
        public string ItemCode { get; set; }
        public bool? IsActive { get; set; }
    }
}
