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
    public class ChildrenTaxes : BaseEntity<int>
    {
        public int TaxId { get;private set; }
        [ForeignKey("TaxId")]
        public Taxes Taxes { get;private set; }
        public string Name { get;private set; }
        public TaxComputation? TaxComputation { get; private set; }
        public decimal Amount { get; private set; }
    }
}
