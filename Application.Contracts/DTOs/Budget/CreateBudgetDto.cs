using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreateBudgetDto
    {
        public int? Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string BudgetName { get; set; }
        [Required]
        public int? CampusId { get; set; }
        [Required]
        public DateTime? From { get; set; }
        [Required]
        public DateTime? To { get; set; }
        [Required]
        public bool? isSubmit { get; set; }
        public virtual List<CreateBudgetLinesDto> BudgetLines { get; set; }
        
    }
}
