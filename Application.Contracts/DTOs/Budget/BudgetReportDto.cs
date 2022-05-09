using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class BudgetReportDto
    {
        public int BudgetId { get; set; }
        public string BudgetName { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public Guid AccountId { get; set; }
        public string Account { get; set; }
        public decimal BudgetAmount { get; set; }
        public decimal IncurredAmount { get; set; }
        public decimal BalanceRemaining { get; set; }
    }
}
