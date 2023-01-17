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
    public class Depreciation : BaseEntity<int>
    {
        [MaxLength(50)]
        public string DocNo { get; private set; }
        [MaxLength(200)]
        public string ModelName { get; private set; }
        public int UseFullLife { get; private set; }
        public Guid AssetAccountId { get; private set; }
        [ForeignKey("AssetAccountId")]
        public Level4 AssetAccount { get; private set; }
        public Guid DepricationExpenseId { get; private set; }
        [ForeignKey("DepricationExpenseId")]
        public Level4 DepricationExpense { get; private set; }
        public Guid AccumulatedDepriciationId { get; private set; }
        [ForeignKey("AccumulatedDepriciationId")]
        public Level4 AccumulatedDepriciation { get; private set; }
        public DepreciationMethod ModelType { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal DecliningRate { get; private set; }

        protected Depreciation()
        {
        }

        public void CreateDocNo()
        {
            //Creating doc no..
            DocNo = "DEP-" + String.Format("{0:000}", Id);
        }
    }
}
