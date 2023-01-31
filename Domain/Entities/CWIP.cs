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
    public class CWIP : BaseEntity<int>
    {
        public DateTime DateOfAcquisition { get; private set; }
        public Guid CWIPAccountId { get; private set; }
        [ForeignKey("CWIPAccountId")]
        public Level4 CWIPAccount { get; private set; }
        public int CostOfAsset { get; private set; }
        public Guid AssetAccountId { get; private set; }
        [ForeignKey("AssetAccountId")]
        public Level4 AssetAccount { get; private set; }
        public int? SalvageValue { get; private set; }
        public bool DepreciationApplicability { get; private set; }
        public int? DepreciationId { get; private set; }
        [ForeignKey("DepreciationId")]
        public Depreciation Depreciation { get; private set; }
        public DepreciationMethod ModelType { get; private set; }
        public Guid? DepreciationExpenseId { get; private set; }
        [ForeignKey("DepreciationExpenseId")]
        public Level4 DepreciationExpense { get; private set; }
        public Guid? AccumulatedDepreciationId { get; private set; }
        [ForeignKey("AccumulatedDepreciationId")]
        public Level4 AccumulatedDepreciation { get; private set; }
        public int? UseFullLife { get; private set; }
        public int Quantinty { get; private set; }
        [Range(0.00, 100.00, ErrorMessage = "Please enter a value between 0 and 100")]
        public decimal? DecLiningRate { get; private set; }
        public bool ProrataBasis { get; private set; }
        public bool Active { get; private set; }

        protected CWIP()
        {

        }
    }
}
