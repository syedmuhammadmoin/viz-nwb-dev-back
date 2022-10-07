using Domain.Constants;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreateEstimatedBudgetDto
    {
        public int? Id { get; set; }
        [Required]
        public int? BudgetId { get; set; }
        [Required]
        [MaxLength(100)]
        public string EstimatedBudgetName { get; set; }
        public virtual List<CreateEstimatedBudgetLinesDto> EstimatedBudgetLines { get; set; }
    }
}
