﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class UpdateBankAccountDto
    {
        [Required]
        public int? Id { get; set; }
        [Required]
        public string AccountNumber { get; set; }
        public string IBAN { get; set; }

        [MaxLength(100)]
        public string AccountTitle { get; set; }
        [Required]
        [MaxLength(10)]
        public string AccountCode { get; set; }
        [Required]
        [MaxLength(80)]
        public string BankName { get; set; }
        [MaxLength(80)]
        public string Branch { get; set; }
        [MaxLength(80)]
        public string Purpose { get; set; }
    }
}
