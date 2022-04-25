using Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class EstimatedBudgetLinesDto
    {
        public int Id { get; set; }
        public Guid AccountId { get; set; }
        public string AccountName { get; set; }
        public CalculationType CalculationType { get; private set; }
        public decimal? Amount { get; private set; }
        public decimal? Percentage { get; private set; }
        public int MasterId { get; set; }
    }
}
