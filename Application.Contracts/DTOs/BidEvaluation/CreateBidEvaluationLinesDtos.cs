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
    public class CreateBidEvaluationLinesDtos
    {
        public int? Id { get; set; }
        [Required]
        public string NameOfBider { get; private set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TechnicalTotal { get; private set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TechnicalObtain { get; private set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal FinancialTotal { get; private set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal FinancialObtain { get; private set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal EvaluatedCost { get; private set; }
        [Required]
        [MaxLength(500)]
        public string Rule { get; private set; }
    }
}
