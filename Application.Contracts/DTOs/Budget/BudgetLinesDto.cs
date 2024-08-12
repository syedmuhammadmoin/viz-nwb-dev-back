using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class BudgetLinesDto
    {
        public int Id { get; set; }
        public string AccountId { get; set; }
        public string AccountName { get; set; }
        public decimal Amount { get; set; }
        public decimal RevisedAmount { get;  set; }
        public decimal IncurredAmount { get; set; }
        public int MasterId { get; set; }
    }
}
