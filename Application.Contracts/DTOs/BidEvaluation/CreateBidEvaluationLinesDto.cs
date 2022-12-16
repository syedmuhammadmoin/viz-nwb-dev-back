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
    public class CreateBidEvaluationLinesDto
    {
        public int? Id { get; set; }
        [Required]
        public string NameOfBider { get;  set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(1, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal? TechnicalTotal { get;  set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(1, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal? TechnicalObtain { get;  set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(1, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal? FinancialTotal { get;  set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(1, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal? FinancialObtain { get;  set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(1, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal? EvaluatedCost { get;  set; }
        [Required]
        [MaxLength(500)]
        public string Rule { get;  set; }
    }
}
