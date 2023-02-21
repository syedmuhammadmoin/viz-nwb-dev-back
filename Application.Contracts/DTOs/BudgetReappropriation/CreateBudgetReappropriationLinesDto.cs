using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreateBudgetReappropriationLinesDto
    {
        public int? Id{ get; set; }
        [Required]
        public Guid? Level4Id { get; set; }
        [Required]
        public int? CampusId { get; set; }
        [Required]
        [MaxLength(500)]
        public string Description { get; set; }
        [Required]
        [Range(0.00, int.MaxValue, ErrorMessage = "Please enter a value between greater than 0")]
        public decimal AdditionAmount { get; set; }
        [Required]
        [Range(0.00, int.MaxValue, ErrorMessage = "Please enter a value between greater than 0")]
        public decimal DeletionAmount { get; set; }
    }
}
