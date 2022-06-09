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
    public class RecordLedger : BaseEntity<int>
    {
        public int TransactionId { get; private set; }
        public Guid Level4_id { get; private set; }
        public int? BusinessPartnerId { get; private set; }
        public int? WarehouseId { get; private set; }
        public int? CampusId { get; private set; }
        [MaxLength(500)]
        public string Description { get; private set; }
        
        public char Sign { get; private set; } // D or C
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; private set; }

        [ForeignKey("Level4_id")]
        public Level4 Level4 { get; private set; }

        [ForeignKey("BusinessPartnerId")]
        public BusinessPartner BusinessPartner { get; private set; }

        [ForeignKey("WarehouseId")]
        public Warehouse Warehouse { get; private set; }

        [ForeignKey("CampusId")]
        public Campus Campus { get; private set; }

        [ForeignKey("TransactionId")]
        public Transactions Transactions { get; private set; }
        public DateTime TransactionDate { get; private set; }
        public DocumentStatus ReconStatus { get; private set; }
        public bool IsReconcilable { get; private set; }


        protected RecordLedger()
        {

        }
       
        public RecordLedger(int transactionId, Guid level4_id, int? businessPartnerId, int? warehouseId, string description, char sign, decimal amount, int? campusId, DateTime transactionDate)
        {
            TransactionId = transactionId;
            Level4_id = level4_id;
            BusinessPartnerId = businessPartnerId;
            WarehouseId = warehouseId;
            Description = description;
            Sign = sign;
            Amount = amount;
            CampusId = campusId;
            TransactionDate = transactionDate;
            ReconStatus = DocumentStatus.Unreconciled;
        }

        public void setIsReconcilable(bool value)
        {
            IsReconcilable = value;
        }
        public void setStatus(DocumentStatus status)
        {
            ReconStatus = status;
        }
    }
}
