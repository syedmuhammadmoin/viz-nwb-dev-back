﻿using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class PettyCashLinesDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string AccountId { get; set; }
        public string AccountName { get; set; }
        public int? BusinessPartnerId { get; set; }
        public string BusinessPartnerName { get; set; }
        public string Description { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public int MasterId { get; set; }
    }
}
