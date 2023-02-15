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
    public class Disposal : BaseEntity<int>
    {
        [MaxLength(50)]
        public string DocNo { get; private set; }
        public int AssetId { get; private set; }
        [ForeignKey("AssetId")]
        public FixedAsset Asset{ get; private set; }
        public int CategoryId { get; private set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal PurchaseCost { get; private set; }
        public int SalvageValue { get; private set; }
        public int UseFullLife { get; private set; }
        public Guid AccumulatedDepreciationId { get; private set; }
        [ForeignKey("AccumulatedDepreciationId")]
        public Level4 AccumulatedDepreciation { get; private set; }
        public DateTime DisposalDate { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal DisposalValue { get; private set; }
        public int WarehouseId { get; private set; }
        [ForeignKey("WarehouseId")]
        public Warehouse Warehouse { get; private set; }
        public int StatusId { get; private set; }
        [ForeignKey("StatusId")]
        public WorkFlowStatus Status { get; private set; }
        public void CreateCode()
        {
            //Creating doc no..
            DocNo = "DIS-" + String.Format("{0:000}", Id);
        }

        public void SetStatus(int statusId)
        {
            StatusId = statusId;
        }
    }
}
