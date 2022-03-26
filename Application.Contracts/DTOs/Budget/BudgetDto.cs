using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class BudgetDto
    {
        public int Id { get; set; }
        public string BudgetName { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public virtual List<BudgetLinesDto> BudgetLines { get; set; }
    }
}
