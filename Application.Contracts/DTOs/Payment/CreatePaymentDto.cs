using Domain.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreatePaymentDto
    {
        public int? Id { get; set; }
        public PaymentType PaymentType { get; set; }
        [Required]
        public int BusinessPartnerId { get; set; }
        public DocType PaymentFormType { get; set; }
        [Required]
        public Guid AccountId { get; set; }
        [Required]
        public DateTime PaymentDate { get; set; }
        [Required]
        public PaymentRegisterType PaymentRegisterType { get; set; }
        [Required]
        public Guid PaymentRegisterId { get; set; }
        [Required]
        [MaxLength(500)]
        public string Description { get; set; }
        public int? CampusId { get; set; }
        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Gross amount must be greater than 0")]
        public decimal GrossPayment { get; set; }
        [Range(0.00, double.MaxValue, ErrorMessage = "Please enter a positive value")]
        public decimal SalesTax { get; set; }
        [Range(0.00, double.MaxValue, ErrorMessage = "Please enter a positive value")]
        public decimal IncomeTax { get; set; }
        [Range(0.00, double.MaxValue, ErrorMessage = "Please enter a positive value")]
        public decimal SRBTax { get;  set; }
        public int? DocumentLedgerId { get; set; }
        [Required]
        public bool isSubmit { get; set; }
    }
}
