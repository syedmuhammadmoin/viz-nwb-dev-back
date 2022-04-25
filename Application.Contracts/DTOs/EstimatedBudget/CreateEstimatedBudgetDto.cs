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
        [MaxLength(100)]
        public string BudgetName { get; set; }
        public CalculationType CalculationType { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Amount { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Percentage { get; private set; }
        [Required]
        public DateTime From { get; set; }
        [Required]
        public DateTime To { get; set; }
        public virtual List<CreateEstimatedBudgetLinesDto> BudgetLines { get; set; }
    }
}
