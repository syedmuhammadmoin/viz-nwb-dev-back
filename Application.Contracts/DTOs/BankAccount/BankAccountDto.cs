using Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class BankAccountDto
    {
        public int Id { get; set; }
        public string DocNo { get; set; }
        public string AccountCode { get; set; }
        public long AccountNumber { get; set; }
        public string AccountTitle { get; set; }
        public BankAccountType BankAccountType { get; set; }
        public string BankName { get; set; }
        public string Branch { get; set; }
        public decimal OpeningBalance { get; set; }
        public DateTime OpeningBalanceDate { get; set; }
        public Guid ChAccountId { get; set; }
        public string ChAccountName { get; set; }
        public Guid ClearingAccountId { get; set; }
        public string ClearingAccount { get; set; }
        public int CampusId { get; set; }
        public string CampusName { get; set; }
        public string Purpose { get; set; }
        public int TransactionId { get; set; }
    }
}
