using Domain.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Disposal : BaseEntity<int>
    {
        [MaxLength(50)]
        public string DocNo { get; private set; }
        
        public int FixedAssetId { get; private set; }
        [ForeignKey("FixedAssetId")]
        public FixedAsset FixedAsset { get; private set; }

        public int ProductId { get; private set; }
        [ForeignKey("ProductId")]
        public Product Product { get; private set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal Cost { get; private set; }
        public int SalvageValue { get; private set; }
        public int UseFullLife { get; private set; }

        public Guid AccumulatedDepreciationId { get; private set; }
        [ForeignKey("AccumulatedDepreciationId")]
        public Level4 AccumulatedDepreciation { get; private set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal BookValue { get; set; }
        public DateTime DisposalDate { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal DisposalValue { get; private set; }

        public int WarehouseId { get; private set; }
        [ForeignKey("WarehouseId")]
        public Warehouse Warehouse { get; private set; }

        public int StatusId { get; private set; }
        [ForeignKey("StatusId")]
        public WorkFlowStatus Status { get; private set; }
        
        protected Disposal()
        {
        }

        public Disposal(int fixedAssetId, int productId, decimal cost, int salvageValue, int useFullLife, Guid accumulatedDepreciationId, decimal bookValue, DateTime disposalDate, decimal disposalValue, int warehouseId, int statusId)
        {
            FixedAssetId = fixedAssetId;
            ProductId = productId;
            Cost = cost;
            SalvageValue = salvageValue;
            UseFullLife = useFullLife;
            AccumulatedDepreciationId = accumulatedDepreciationId;
            BookValue = bookValue;
            DisposalDate = disposalDate;
            DisposalValue = disposalValue;
            WarehouseId = warehouseId;
            StatusId = statusId;
        }

        public void Update(int fixedAssetId, int productId, decimal cost, int salvageValue, int useFullLife, Guid accumulatedDepreciationId, decimal bookValue, DateTime disposalDate, decimal disposalValue, int warehouseId, int statusId)
        {
            FixedAssetId = fixedAssetId;
            ProductId = productId;
            Cost = cost;
            SalvageValue = salvageValue;
            UseFullLife = useFullLife;
            AccumulatedDepreciationId = accumulatedDepreciationId;
            BookValue = bookValue;
            DisposalDate = disposalDate;
            DisposalValue = disposalValue;
            WarehouseId = warehouseId;
            StatusId = statusId;
        }

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
