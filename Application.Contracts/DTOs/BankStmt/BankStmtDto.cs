using Domain.Constants;
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
        //public string DocNo { get; set; }
        public DateTime DocDate { get; set; }
        //public decimal Amount { get; set; }
        //public decimal ReconciledAmount { get; set; }
        //public decimal UnreconciledAmount { get; set; }
        public ReconStatus BankReconStatus { get; set; }
    }
}
