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
        public Types Type { get;  set; }
        public string BankAcountId { get;  set; }
        public string BankName { get;  set; }
        public string AccountNumber { get;  set; }
        public string SuspenseAccount { get;  set; }
        public string ProfitAccount { get;  set; }
        public string LossAccount { get;  set; }
        public string CashAccount { get;  set; }
        public int OrganizationId { get; set; }
    }
}
