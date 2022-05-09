﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class EstimatedBudgetDto
    {
        public int Id { get; set; }
        public int BudgetId { get; set; }
        public string EstimatedBudgetName { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public virtual List<EstimatedBudgetLinesDto> EstimatedBudgetLines { get; set; }
    }
}