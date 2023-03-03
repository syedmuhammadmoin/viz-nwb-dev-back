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
    public class CreateBudgetReappropriationDto
    {
        public int? Id { get; set; }
        [Required]
        public int? BudgetId { get; set; }  
        [Required]
        public DateTime BudgetReappropriationDate { get; set;}
        [Required]
        public bool IsSubmit { get; set;}
        [Required]
        public virtual List<CreateBudgetReappropriationLinesDto> BudgetReappropriationLines { get; set; }
    }
}
