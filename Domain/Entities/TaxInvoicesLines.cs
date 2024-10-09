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
    public class TaxInvoicesLines : BaseEntity<int>
    {
        public TaxBase? TaxBase { get; private set; }
        public string? AccountId { get; private set; }
        [ForeignKey("AccountId")]
        public Level4? Account {  get; private set; }
        public int TaxesId { get; private set; }
        [ForeignKey("TaxesId")]
        public Taxes Taxes { get; private set; }
    }
}
