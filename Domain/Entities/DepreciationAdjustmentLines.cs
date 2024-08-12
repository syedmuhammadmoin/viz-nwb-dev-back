using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class DepreciationAdjustmentLines : BaseEntity<int>
    {
        public int FixedAssetId { get; private set; }
        [ForeignKey("FixedAssetId")]
        public FixedAsset FixedAsset { get; private set; }
        public string Level4Id { get; private set; }
        [ForeignKey("Level4Id")]
        public Level4 Level4 { get; private set; }
        public string Description { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Debit { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Credit { get; private set; }
        public bool IsActive { get; private set; }
        public int MasterId { get; private set; }
        [ForeignKey("MasterId")]
        public DepreciationAdjustmentMaster Master { get; private set; }

        protected DepreciationAdjustmentLines()
        {

        }
    }
}
