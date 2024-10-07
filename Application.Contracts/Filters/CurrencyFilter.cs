using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Filters
{
    public class CurrencyFilter:  PaginationFilter
    {
        public string Code { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public DateTime? LastUpdate { get; set; }
        public decimal? UnitPerUSD { get; set; }
    }
}
