using Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs.TaxSetting
{
    public record TaxSettingDto
    {
        public int? SalesTaxId { get; set; }
        public int? PurchaseTaxId { get; set; }
        public Periodicity Periodicity { get; set; }
        public int RemindPeriod { get; set; }
        public string? JournalAccountId { get; set; }
        public bool RoundPerLine { get; set; }
        public bool RoundGlobally { get; set; }
        public bool EuropeVAT { get; set; }
        public int? CountryId { get; set; }
    }
}
