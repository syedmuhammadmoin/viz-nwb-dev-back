using Domain.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreateBankAccountDto
    {
        [Required]
        public long? AccountNumber { get; set; }
        [Required]
        [MaxLength(50)]
        public string AccountTitle { get; set; }
        [Required]
        [MaxLength(10)]
        public string AccountCode { get; set; }
        [Required]
        public BankAccountType? BankAccountType { get; set; }
        [Required]
        [MaxLength(50)]
        public string BankName { get; set; }
        [MaxLength(50)]
        public string Branch { get; set; }
        [Required]
        [Range(0.00, float.MaxValue, ErrorMessage = "Please enter a positive value")]
        public decimal? OpeningBalance { get; set; }
        [Required]
        public DateTime? OpeningBalanceDate { get; set; }
        [MaxLength(200)]
        public string Purpose { get; set; }
        [Required]
        public int? CampusId { get; set; }
    }
}
