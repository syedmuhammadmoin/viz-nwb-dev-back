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
    public class DepreciationRegister : BaseEntity<int>
    {
        public int FixedAssetId { get; private set; }
        [ForeignKey("FixedAssetId")]
        public FixedAsset FixedAsset { get; private set; }
        public DateTime TransactionDate { get; set; }

        public bool IsAutomatedCalculation { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal DepreciationAmount { get; private set; }
        public string Description { get; private set; }

        
        protected DepreciationRegister()
        {

        }
    }
}
