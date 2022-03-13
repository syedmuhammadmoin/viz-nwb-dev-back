using Application.Contracts.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreateBankStmtDto
    {
        public int? Id { get; set; }
        [Required]
        public int BankAccountId { get; set; }
        [Required]
        [MaxLength(100)]
        public string Description { get; set; }
        [Required]
        public decimal OpeningBalance { get; set; }
        [Required]
        public virtual List<CreateBankStmtLinesDto> BankStmtLines { get; set; }
    }
}
