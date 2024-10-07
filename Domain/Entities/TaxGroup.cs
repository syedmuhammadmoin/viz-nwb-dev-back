using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class TaxGroup : BaseEntity<int>
    {
        public string? Name { get;private set; }
        public int? CountryId { get; private set; }
        [ForeignKey("CountryId")]
        public Country? Country { get;private set; }
        public string? Company {  get; private set; }
        public int? Sequence {  get; private set; }
        public string? PayableAccountId { get; private set; }
        [ForeignKey("PayableAccountId")]
        public Level4? PayableAccount {  get; private set; }
        public string? ReceivableAccountId { get; private set; }
        [ForeignKey("ReceivableAccountId")]
        public Level4? ReceivableAccount {  get; private set; }
        public string? AdvanceAccountId { get; private set; }
        [ForeignKey("AdvanceAccountId")]
        public Level4? AdvanceAccount {  get; private set; }
        public decimal? PreceedingTtl { get; private set; }
        public ICollection<Taxes> Taxes { get; set; }

    }
}
