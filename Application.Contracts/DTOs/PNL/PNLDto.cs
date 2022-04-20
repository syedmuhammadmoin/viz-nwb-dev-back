using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class PNLDto
    {
        public string Transactional { get; set; }
        public string BusinessPartnerName { get; set; }
        public string WarehouseName { get; set; }
        public string CampusName { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal Balance { get; set; }
    }
}
