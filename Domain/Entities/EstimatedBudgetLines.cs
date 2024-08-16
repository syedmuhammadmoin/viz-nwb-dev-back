using Domain.Base;
using Domain.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class EstimatedBudgetLines : BaseEntity<int>
    {
        public string AccountId { get; private set; }
        [ForeignKey("AccountId")]
        public Level4 Account { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; private set; }
        public CalculationType CalculationType { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Value { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal EstimatedValue { get; private set; }
        public int MasterId { get; private set; }
        [ForeignKey("MasterId")]
        public EstimatedBudgetMaster EstimatedBudgetMaster { get; private set; }
        protected EstimatedBudgetLines()
        {

        }
    }
}
