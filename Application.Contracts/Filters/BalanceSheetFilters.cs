using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Filters
{
    public class BalanceSheetFilters
    {
        public DateTime DocDate { get; set; }
        public string AccountName { get; set; }
        public string CampusName { get; set; }
    }
}
