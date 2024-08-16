using Domain.Base;
using Domain.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class DepreciationModel : BaseEntity<int>
    {
        [MaxLength(200)]
        public string ModelName { get; private set; }
        public int UseFullLife { get; private set; }

        public string AssetAccountId { get; private set; }
        [ForeignKey("AssetAccountId")]
        public Level4 AssetAccount { get; private set; }
        
        public string DepreciationExpenseId { get; private set; }
        [ForeignKey("DepreciationExpenseId")]
        public Level4 DepreciationExpense { get; private set; }
        
        public string AccumulatedDepreciationId { get; private set; }
        [ForeignKey("AccumulatedDepreciationId")]
        public Level4 AccumulatedDepreciation { get; private set; }
        
        public DepreciationMethod ModelType { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal DecliningRate { get; private set; }

        protected DepreciationModel()
        {
        }
    }
}
