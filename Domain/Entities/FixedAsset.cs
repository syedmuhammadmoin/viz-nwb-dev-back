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
    public class FixedAsset : BaseEntity<int>
    {
        [MaxLength(20)]
        public string AssetCode { get; private set; }
        public DateTime DateofAcquisition { get; private set; }
        [MaxLength(100)]
        public string Name { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal PurchaseCost { get; private set; }
        public int CategoryId { get; private set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; private set; }
        public int CampusId { get; private set; }
        [ForeignKey("CampusId")]
        public Campus Campus { get; private set; }
        public int WarehouseId { get; private set; }
        [ForeignKey("WarehouseId")]
        public Warehouse Warehouse { get; private set; }
        public int SalvageValue { get; private set; }
        public bool DepreciationApplicability { get; private set; }
        public int? DepreciationId { get; private set; }
        [ForeignKey("DepreciationId")]
        public Depreciation Depreciation { get; private set; }
        public DepreciationMethod ModelType { get; private set; }
        public Guid? AssetAccountId { get; private set; }
        [ForeignKey("AssetAccountId")]
        public Level4 AssetAccount { get; private set; }
        public Guid? DepreciationExpenseId { get; private set; }
        [ForeignKey("DepreciationExpenseId")]
        public Level4 DepreciationExpense { get; private set; }
        public Guid? AccumulatedDepreciationId { get; private set; }
        [ForeignKey("AccumulatedDepreciationId")]
        public Level4 AccumulatedDepreciation { get; private set; }
        public int? UseFullLife { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal DecLiningRate { get; private set; }
        public bool ProrataBasis { get; private set; }
        public bool Active { get; private set; }
        public int StatusId { get; private set; }
        [ForeignKey("StatusId")]
        public WorkFlowStatus Status { get; private set; }
        protected FixedAsset()
        {
        }
        public void SetStatus(int statusId)
        {
            StatusId = statusId;
        }

        public void CreateCode()
        {
            //Creating doc no..
            AssetCode = "FXA-" + String.Format("{0:000}", Id);
        }
    }
}
