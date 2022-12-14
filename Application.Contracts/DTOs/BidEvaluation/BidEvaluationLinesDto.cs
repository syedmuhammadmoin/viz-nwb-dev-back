using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs.BidEvaluation
{
    public class BidEvaluationLinesDto
    {
        public int Id { get; set; }
        public string NameOfBider { get; set; }
        public decimal TechnicalTotal { get; set; }
        public decimal TechnicalObtain { get;  set; }
        public decimal FinancialTotal { get; set; }
        public decimal FinancialObtain { get; set; }
        public decimal EvaluatedCost { get; set; }
        public string Rule { get; set; }
        public int MasterId { get; set; }
    }
}
