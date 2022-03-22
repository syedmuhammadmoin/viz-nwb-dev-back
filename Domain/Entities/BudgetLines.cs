using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class BudgetLines : BaseEntity<int>
    {
        public Guid AccountId { get; private set; }
        [ForeignKey("AccountId")]
        public Level4 Account { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; private set; }
        public int MasterId { get; private set; }
        [ForeignKey("MasterId")]
        [JsonIgnore]
        public BudgetMaster BudgetMaster { get; private set; }
    }
}
