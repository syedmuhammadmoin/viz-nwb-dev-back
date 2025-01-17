﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
        [Range(0, double.MaxValue, ErrorMessage = "Value cannot be negative")]
        public decimal? TechnicalTotal { get;  set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Value cannot be negative")]
        public decimal? TechnicalObtain { get;  set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Value cannot be negative")]
        public decimal? FinancialTotal { get;  set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Value cannot be negative")]
        public decimal? FinancialObtain { get;  set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Value cannot be negative")]
        public decimal? EvaluatedCost { get;  set; }
        [Required]
        [MaxLength(500)]
        public string Rule { get;  set; }
    }
}
