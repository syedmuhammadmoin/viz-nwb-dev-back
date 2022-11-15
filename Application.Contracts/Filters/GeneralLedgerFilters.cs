using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Filters
{
    public class GeneralLedgerFilters
    {
        public string AccountName { get; set; }
        [Required]
        public DateTime? DocDate { get; set; }
        [Required]
        public DateTime? DocDate2 { get; set; }
        public int? BusinessPartnerId { get; set; }
        public string WarehouseName { get; set; }
        public string CampusName { get; set; }
    }
}
