using Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CurrencyDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Symbol { get;  set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public string SubUnit { get; set; }
        public DateTime? LastUpdate { get; set; }
        public decimal UnitPerUSD { get; set; }
        public decimal? USDPerUnit { get; set; }

        public IEnumerable<CurrencyLineDto> CurrencyLines { get; set; }
        int OrganizationId { get; set; }
    }
}
