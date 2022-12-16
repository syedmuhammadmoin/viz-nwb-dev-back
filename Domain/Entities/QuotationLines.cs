using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class QuotationLines : BaseEntity<int>
    {

        public int? ItemId { get; private set; }
        [ForeignKey("ItemId")]
        public Product Item { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; private set; }
        [MaxLength(500)]
        public string Description { get; private set; }
        public int  Quantity { get; private set; }
        public int MasterId { get; private set; }
        [ForeignKey("MasterId")]
        public QuotationMaster QuotationMaster { get; private set; }

        protected QuotationLines()
        {
        }
    }
}
