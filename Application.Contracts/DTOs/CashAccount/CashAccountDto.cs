using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CashAccountDto
    {
        public int Id { get; set; }
        public string CashAccountName { get; set; }
        public string Handler { get; set; }
        public decimal OpeningBalance { get; set; }
        public DateTime OpeningBalanceDate { get; set; }
        public string DocNo { get; set; }
        public Guid ChAccountId { get; set; }
        public String ChAccountName { get; set; }
        public int CampusId { get; set; }
        public string CampusName { get; set; }
        public int TransactionId { get; set; }
    }
}
