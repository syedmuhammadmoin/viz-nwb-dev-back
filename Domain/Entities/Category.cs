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
    public class Category : BaseEntity<int>
    {
        [MaxLength(100)]
        public string Name { get; private set; }
        //Will Restrict to Only Inventory Account
        public Guid InventoryAccountId { get; private set; }
        //Will Restrict to Only Revenue Account
        public Guid RevenueAccountId { get; private set; }
        //Will Restrict to Only Cost Account
        public Guid CostAccountId { get; private set; }
        [ForeignKey("InventoryAccountId")]
        public Level4 InventoryAccount { get; private set; }
        [ForeignKey("RevenueAccountId")]
        public Level4 RevenueAccount { get; private set; }
        [ForeignKey("CostAccountId")]
        public Level4 CostAccount { get; private set; }
        public bool IsFixedAsset { get; private set; }
        public int? DepreciationModelId { get; private set; }
        [ForeignKey("DepreciationModelId")]
        public DepreciationModel DepreciationModel { get; private set; }
        
        protected Category()
        {
        }

        public void DepreciationIdnull()
        {
            DepreciationModelId = null;
        }

    }
}
