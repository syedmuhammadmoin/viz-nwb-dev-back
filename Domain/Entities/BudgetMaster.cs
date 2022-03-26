using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class BudgetMaster : BaseEntity<int>
    {
        [MaxLength(100)]
        public string BudgetName { get; private set; }
        public DateTime From { get; private set; }
        public DateTime To { get; private set; }
        public virtual List<BudgetLines> BudgetLines { get; private set; }
    }
}
