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
    public class IssuanceReturnLines : BaseEntity<int>
    {
        public int ItemId { get; private set; }
        [ForeignKey("ItemId")]
        public Product Item { get; private set; }
        [MaxLength(500)]
        public string Description { get; private set; }
        public int Quantity { get; private set; }
        public DocumentStatus Status { get; private set; }
        public int WarehouseId { get; private set; }
        [ForeignKey("WarehouseId")]
        public Warehouse Warehouse { get; private set; }
        public int MasterId { get; private set; }
        [ForeignKey("MasterId")]
        public IssuanceReturnMaster IssuanceReturnMaster { get; private set; }
        public int? FixedAssetId { get; private set; }
        [ForeignKey("FixedAssetId")]
        public FixedAsset Asset { get; private set; }


        public void SetStatus(DocumentStatus status)
        {
            Status = status;
        }

        protected IssuanceReturnLines()
        {

        }
    }
}
