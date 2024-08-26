using Domain.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class JournalDto
    {
        public int Id { get; set; }
        public string Name { get;  set; }
        public JournalTypes JournalType { get;  set; }
        public int? BankAcountId { get;  set; }
        public string? BankNameId { get;  set; }
        public string? AccountNumberId { get;  set; }
        public string? SuspenseAccountId { get;  set; }
        public string? ProfitAccountId { get;  set; }
        public string? LossAccountId { get;  set; }
        public string? CashAccountId { get;  set; }
        public string? DefaultAccountId { get;  set; }
        public string? BankAccount { get; set; }     
        public string? BankName { get; set; }
        public string? AccountNumber { get; set; } 
        public string? SuspenseAccount { get; set; }    
        public string? ProfitAccount { get; set; }
        public string? LossAccount { get; set; }
        public string? CashAccount { get; set; }
        public string? DefaultAccount { get; set; }
        public int OrganizationId { get; set; }
    }
}
