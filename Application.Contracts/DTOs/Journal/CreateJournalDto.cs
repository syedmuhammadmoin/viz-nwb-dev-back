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
        public string BankAcountId { get;  set; }
       // [Required]
        [MaxLength(50)]
        public string? BankName { get;  set; }
      //  [Required]
        [MaxLength(50)]
        public string? AccountNumber { get;  set; }
       // [Required]
        public string? SuspenseAccount { get;  set; }
       // [Required]
        public string? ProfitAccount { get;  set; }
        //[Required]
        public string? LossAccount { get;  set; }
       // [Required]
        public string? CashAccount { get;  set; }
        public string? DefaultAccount { get;  set; }
        public int OrganizationId { get; set; }
    }
}
