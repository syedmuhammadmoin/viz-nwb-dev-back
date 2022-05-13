using Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class UnReconStmtDto
    {
        public int Id { get; set; }
        public string DocNo { get; set; }
        public DateTime DocDate { get; set; }
        public decimal Amount { get; set; }
        public decimal ReconciledAmount { get; set; }
        public decimal UnreconciledAmount { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public DocumentStatus BankReconStatus { get; set; }
    }
}
