﻿using Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class BankStmtDto
    {
        public int Id { get; set; }
        public string BankAccountName { get; set; }
        public int BankAccountId { get; set; }
        public DateTime DocDate { get; set; }
        public DocumentStatus BankReconStatus { get; set; }
        public decimal OpeningBalance { get; set; }
        public string Description { get; set; }
        public virtual List<BankStmtLinesDto> BankStmtLines { get; set; }
    }
}
