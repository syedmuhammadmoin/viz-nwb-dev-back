using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Filters
{
    public class GeneralLedgerFilters
    {
        public string AccountName { get; set; }
        public DateTime DocDate { get; set; }
        public DateTime DocDate2 { get; set; }
        public string BusinessPartnerName { get; set; }
        public string WarehouseName { get; set; }
        public string CampusName { get; set; }
    }
}
