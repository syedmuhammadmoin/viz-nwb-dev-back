using Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CurrencyLineDto
    {
        public int Id { get; set; }
        public int CurrencyId { get; set; }
        public DateTime Date { get; set; }
        public decimal UnitPerUSD { get; set; }
        public decimal USDPerUnit { get; set; }
        int OrganizationId { get; set; }
    }
}
