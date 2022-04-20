using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Filters
{
    public class PNLFilters
    {
        public string AccountName { get; set; }
        [Required]
        public DateTime DocDate { get; set; }
        [Required]
        public DateTime DocDate2 { get; set; }
        public string BusinessPartner { get; set; }
        public string Warehouse { get; set; }
        public string Location { get; set; }
    }
}
