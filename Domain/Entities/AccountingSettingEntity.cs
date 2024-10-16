using Domain.Base;
using Domain.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AccountingSettingEntity : BaseEntity<int>
    {
        public string? LastMonth { get; private set; }
        public int? LastDay { get; private set; }
        public DateTime? ThresholdDate { get; private set; }
        public bool DyanmicReports { get; private set; }
        public int? CurrencyId { get; private set; }
        [ForeignKey("CurrencyId")]
        public Currency? Currency { get; private set; }
        public int? OrganizationId { get; private set; }
        [ForeignKey("OrganizationId")]
        public Organization? Organization { get; private set; }
        public int? SalesTaxId { get; private set; }
        [ForeignKey("SalesTaxId")]
        public Taxes? SalesTax { get; private set; }
        public int? PurchaseTaxId { get; private set; }
        [ForeignKey("PurchaseTaxId")]
        public Taxes PurchaseTax { get; private set; }
        public Periodicity? Periodicity { get; private set; }
        public int? RemindPeriod { get; private set; }      
        public string? JournalAccountId { get; private set; }
        [ForeignKey("JournalAccountId")]
        public Level4? JournalAccount { get; private set; }
        public string? TaxCashBasisId { get; private set; }
        [ForeignKey("TaxCashBasisId")]
        public Level4? TaxCashBasis { get; private set; }
        public string? BaseTaxReceivedAccountId { get; private set; }
        [ForeignKey("BaseTaxReceivedAccountId")]
        public Level4? BaseTaxReceivedAccount {  get; private set; }
        public bool? RoundPerLine { get; private set; }
        public bool? RoundGlobally { get; private set; }
        public bool? EuropeVAT { get; private set; }
        public int? CountryId { get; private set; }
        [ForeignKey("CountryId")]
        public Country Country { get; private set; }
    }
}
