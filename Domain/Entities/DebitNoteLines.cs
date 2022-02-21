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
    public class DebitNoteLines : BaseEntity<int>
    {
        public int? ItemId { get; private set; }
        [ForeignKey("ItemId")]
        public Product Item { get; private set; }
        [MaxLength(500)]
        public string Description { get; private set; }
        public int Quantity { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Cost { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Tax { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal SubTotal { get; private set; }
        public Guid AccountId { get; private set; }
        [ForeignKey("AccountId")]
        public Level4 Account { get; private set; }
        public int LocationId { get; private set; }
        [ForeignKey("LocationId")]
        public Location Location { get; private set; }
        public int MasterId { get; private set; }
        [ForeignKey("MasterId")]
        public DebitNoteMaster DebitNoteMaster { get; private set; }

        protected DebitNoteLines()
        {

        }
    }
}
