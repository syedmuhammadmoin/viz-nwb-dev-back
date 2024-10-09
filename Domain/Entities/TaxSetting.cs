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
    public class TaxSetting : BaseEntity<int>
    {
        public int? SalesTaxId { get;private set; }
        [ForeignKey("SalesTaxId")]
        public Taxes? SalesTax { get;private set; }
        public int? PurchaseTaxId { get;private set; }
        [ForeignKey("PurchaseTaxId")]
        public Taxes PurchaseTax { get;private set; }
        public Periodicity? Periodicity { get;private set; }
        public int? RemindPeriod { get;private set; }
      //  public int? JournalAccountId { get;private set; }
        public string? JournalAccountId { get;private set; }
        [ForeignKey("JournalAccountId")]
        public Level4 JournalAccount {  get;private set; }
        public bool? RoundPerLine { get;private set; }
        public bool? RoundGlobally { get;private set; }
        public bool? EuropeVAT {  get;private set; }
        public int? CountryId { get;private set; }
        public Country Country { get;private set; }
    }
}
