﻿using Domain.Base;
using Domain.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class EstimatedBudgetLines : BaseEntity<int>
    {
        public Guid AccountId { get; private set; }
        [ForeignKey("AccountId")]
        public Level4 AccountName { get; private set; }
        public CalculationType CalculationType { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Amount { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Percentage { get; private set; }
        public int MasterId { get; private set; }
        [ForeignKey("MasterId")]
        public EstimatedBudgetMaster EstimatedBudgetMaster { get; private set; }
        protected EstimatedBudgetLines()
        {

        }
    }
}
