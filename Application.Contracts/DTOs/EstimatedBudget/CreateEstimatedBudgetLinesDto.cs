using Domain.Constants;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreateEstimatedBudgetLinesDto
    {
        public int? Id { get; set; }
        [Required]
        public Guid AccountId { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.00, float.MaxValue, ErrorMessage = "Please enter a positive value")]
        public decimal? Amount { get; set; }
        [Required]
        public CalculationType CalculationType { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.00, float.MaxValue, ErrorMessage = "Please enter a positive value")]
        public decimal? Value { get; set; }
    }
}
