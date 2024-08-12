using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class TrialBalanceDto
    {
        public string AccountId { get; set; }
        public DateTime DocDate { get; set; }
        public string AccountName { get; set; }
        public decimal DebitOB { get; set; }
        public decimal CreditOB { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal DebitCB { get; set; }
        public decimal CreditCB { get; set; }
    }
}
