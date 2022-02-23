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
    public class RecordLedger : BaseEntity<int>
    {
        public int TransactionId { get; private set; }
        public Guid Level4_id { get; private set; }
        public int? BusinessPartnerId { get; private set; }
        public int? LocationId { get; private set; }
        [MaxLength(500)]
        public string Description { get; private set; }
        
        public char Sign { get; private set; } // D or C
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; private set; }

        [ForeignKey("Level4_id")]
        public Level4 Level4 { get; private set; }

        [ForeignKey("BusinessPartnerId")]
        public BusinessPartner BusinessPartner { get; private set; }

        [ForeignKey("LocationId")]
        public Location Location { get; private set; }

        [ForeignKey("TransactionId")]
        public Transactions Transactions { get; private set; }

        protected RecordLedger()
        {

        }
        public RecordLedger(int transactionId, Guid level4_id, int? businessPartnerId, int? locationId, string description, char sign, decimal amount)
        {
            TransactionId = transactionId;
            Level4_id = level4_id;
            BusinessPartnerId = businessPartnerId;
            LocationId = locationId;
            Description = description;
            Sign = sign;
            Amount = amount;
        }
    }
}
