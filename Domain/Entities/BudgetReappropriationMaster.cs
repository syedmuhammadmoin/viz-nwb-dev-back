using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class BudgetReappropriationMaster : BaseEntity<int>
    {
        public int BudgetId { get; private set; }
        [ForeignKey("BudgetId")]
        public BudgetMaster Budget { get; private set; }
        public DateTime BudgetReappropriationDate { get; private set; }
        public int StatusId { get; private set; }
        [ForeignKey("StatusId")]
        public WorkFlowStatus Status { get; private set; }
        public virtual List<BudgetReappropriationLines> BudgetReappropriationLines { get; private set; }
        protected BudgetReappropriationMaster()
        {

        }
        public void SetStatus(int statusId)
        {
            StatusId = statusId;
        }
    }
}
