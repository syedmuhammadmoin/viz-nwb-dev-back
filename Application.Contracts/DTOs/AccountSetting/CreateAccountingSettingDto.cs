using Domain.Constants;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs.AccountSetting
{
    public class CreateAccountingSettingDto
    {
        public int? Id { get; set; }
        public int? LastMonth { get; set; }
        public int? LastDay { get; set; }
        public DateTime? ThresholdDate { get; set; }
        public bool DyanmicReports { get; set; }
        public int? CurrencyId { get; set; }
        public int? OrganizationId { get; set; }
        public int? SalesTaxId { get; set; }
        public int? PurchaseTaxId { get; set; }
        public Periodicity? Periodicity { get; set; }
        public int? RemindPeriod { get; set; }
        public string? JournalAccountId { get; set; }
        public string? TaxCashBasisId { get; set; }
        public string? BaseTaxReceivedAccountId { get; set; }
        public bool? RoundPerLine { get; set; }
        public bool? RoundGlobally { get; set; }
        public bool? EuropeVAT { get; set; }
        public int? CountryId { get; set; }
        public string? Environment { get; set; }
        public string? ApiId { get; set; }
        public string? ApiKey { get; set; }
        public string? CompanyCode { get; set; }
        public bool? UseUPC { get; set; }
        public bool? CommitTransactions { get; set; }
        public bool? AddressValidation { get; set; }
        public bool? IsAvatax { get; set; }
        public bool? CaseBasis { get; set; }
    }
}
