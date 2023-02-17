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
    public class GRNLines : BaseEntity<int>
    {
        public int ItemId { get; private set; }
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
        public DocumentStatus Status { get; private set; }
        public int WarehouseId { get; private set; }
        [ForeignKey("WarehouseId")]
        public Warehouse Warehouse { get; private set; }
        public int MasterId { get; private set; }
        [ForeignKey("MasterId")]
        public GRNMaster GRNMaster { get; private set; }
        public bool IsFixedAssetCreated { get; private set; }
        public void UpdateIsFixedAssetCreated(bool isFixedAssetCreated)
        {
            IsFixedAssetCreated = isFixedAssetCreated;
        }
        public void SetStatus(DocumentStatus status)
        {
            Status = status;
        }
        protected GRNLines()
        {

        }


    }
}
