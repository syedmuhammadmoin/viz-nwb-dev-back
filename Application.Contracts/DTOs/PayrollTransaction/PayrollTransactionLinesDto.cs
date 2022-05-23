﻿using Domain.Constants;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class PayrollTransactionLinesDto
    {
        public int Id { get; set; }
        public int PayrollItemId { get; set; }
        public string PayrollItem { get; set; }
        public PayrollType PayrollType { get; set; }
        public decimal Amount { get; set; }
        public Guid AccountId { get; set; }
        public string Account { get; set; }
        public int MasterId { get; set; }
    }
}
