using Domain.Base;
using Domain.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Taxes : BaseEntity<int>
    {
        [MaxLength(80)]
        public string Name { get; private set; }
        public TaxType TaxType { get; private set; }
        public TaxComputation? TaxComputation { get; private set; }
        public string? AccountId { get; private set; }
        [ForeignKey("AccountId")]
        public Level4 Account { get; private set; }
        public decimal? Amount { get; private set; }
        public TaxScope? TaxScope { get; private set; }
        public virtual List<TaxInvoicesLines> TaxInvoicesLines { get; private set; }
        public virtual List<TaxRefundLines> TaxRefundLines { get; private set; }
        public string? Description { get; private set; }
        public string? LegalNotes { get; private set; }
        public decimal Percent { get; private set; }
        public virtual List<ChildrenTaxes> ChildrenTaxes { get; private set; } = null;
        public Taxes(int id, string name, TaxType taxType)
        {
            Id = id;
            Name = name;
            TaxType = taxType;
        }

        protected Taxes()
        {

        }
    }
}
