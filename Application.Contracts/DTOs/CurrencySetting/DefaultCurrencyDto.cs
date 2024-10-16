using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs.CurrencySetting
{
    public record DefaultCurrencyDto
    {
        public int Id { get; set; }
        public int? CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public int? OrganizationId { get; set; }
        public string OrganizationName { get; set; }

    }
}
