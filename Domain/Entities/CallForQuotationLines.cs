using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class CallForQuotationLines : BaseEntity<int>
    {
        public int? ItemId { get; private set; }
        [ForeignKey("ItemId")]
        public Product Item { get; private set; }
        [MaxLength(500)]
        public string Description { get; private set; }
        public int Quantity { get; private set; }
        public int MasterId { get; private set; }
        [ForeignKey("MasterId")]
        public CallForQuotationMaster CallForQuotation { get; private set; }

        protected CallForQuotationLines()
        {
        }
    }
}
