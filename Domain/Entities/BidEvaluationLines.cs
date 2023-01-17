using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class BidEvaluationLines : BaseEntity<int>
    {
        public string NameOfBider { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TechnicalTotal { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TechnicalObtain { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal FinancialTotal { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal FinancialObtain { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal EvaluatedCost { get; private set; }
        [MaxLength(500)]
        public string Rule { get; private set; }
        public int MasterId { get; private set; }
        [ForeignKey("MasterId ")]
        public BidEvaluationMaster BidEvaluationMaster { get; private set; }

        protected BidEvaluationLines()
        {

        }
    }
}
