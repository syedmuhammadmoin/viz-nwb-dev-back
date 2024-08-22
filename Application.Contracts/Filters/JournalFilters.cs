using Domain.Constants;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Filters
{
    public class JournalFilters
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public Types Types { get; set; }
        public string BankAcountId { get; private set; }
        [Required]
        [MaxLength(50)]
        public string BankName { get; private set; }
        [Required]
        [MaxLength(50)]
        public string AccountNumber { get; private set; }
        [Required]
        public string SuspenseAccount { get; private set; }
        [Required]
        public string ProfitAccount { get; private set; }
        [Required]
        public string LossAccount { get; private set; }
        [Required]
        public string CashAccount { get; private set; }
    }
}
