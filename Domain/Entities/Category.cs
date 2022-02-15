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
        public Category(Category category)
        {
            Name = category.Name;
            InventoryAccountId = category.InventoryAccountId;
            RevenueAccount = category.RevenueAccount;
            CostAccount = category.CostAccount;
            InventoryAccount = category.InventoryAccount;
            RevenueAccount = category.RevenueAccount;    
        }
        protected Category()
        {

        }
    }
}
