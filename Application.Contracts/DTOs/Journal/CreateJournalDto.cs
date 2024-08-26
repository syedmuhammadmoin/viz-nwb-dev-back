using Domain.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreateJournalDto
    {
        public int? Id { get; set; }
        [Required(ErrorMessage = "Journal Name")]
        public string Name { get;  set; }
        [Required]
        public JournalTypes JournalType { get;  set; }
        public int? BankAcountId { get;  set; }
        [MaxLength(50)]
        public string? BankNameId { get;  set; }
        [MaxLength(50)]
        public string? AccountNumberId { get;  set; }
        public string? SuspenseAccountId { get;  set; }
        public string? ProfitAccountId { get;  set; }
        public string? LossAccountId { get;  set; }
        public string? CashAccountId { get;  set; }
        public string? DefaultAccountId { get;  set; }
        public int OrganizationId { get; set; }
    }
}
