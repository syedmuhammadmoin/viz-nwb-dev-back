using Domain.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreatePayrollPaymentDto
    {
        public DateTime PaymentDate { get; set; }
        public PaymentRegisterType PaymentRegisterType { get; set; } // 1 = cashAccount, 2 = BankAccount
        [Required]
        public string? PaymentRegisterId { get; set; }
        [MaxLength(20)]
        public string ChequeNo { get; set; }
        [Required]
        public string Description { get; set; }
        public List<CreatePayrollPaymentLinesDto> CreatePayrollTransLines { get; set; }
    }
}
