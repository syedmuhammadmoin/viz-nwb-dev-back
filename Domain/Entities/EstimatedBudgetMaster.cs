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
    public class EstimatedBudgetMaster : BaseEntity<int>
    {
        [MaxLength(100)]
        public string EstimatedBudgetName { get; private set; }
        public int BudgetId { get; private set; }
        [ForeignKey("BudgetId")]
        public BudgetMaster PreviousBudget { get; private set; }
        public DateTime From { get; private set; }
        public DateTime To { get; private set; }
        public virtual List<EstimatedBudgetLines> EstimatedBudgetLines { get; private set; }

        protected EstimatedBudgetMaster()
        {

        }
    }
}
