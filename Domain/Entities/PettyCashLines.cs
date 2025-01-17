﻿using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class PettyCashLines : BaseEntity<int>
    {
        public DateTime Date { get; private set; }

        public string AccountId { get; private set; }
        [ForeignKey("AccountId")]
        public Level4 Account { get; private set; }
        public int? BusinessPartnerId { get; private set; }
        [ForeignKey("BusinessPartnerId")]
        public BusinessPartner BusinessPartner { get; private set; }
        [MaxLength(500)]
        public string Description { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Debit { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Credit { get; private set; }
        
        public int MasterId { get; private set; }
        [ForeignKey("MasterId ")]
        public PettyCashMaster PettyCashMaster { get; private set; }

        protected PettyCashLines()
        {
        }


    }
}
