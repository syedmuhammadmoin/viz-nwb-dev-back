﻿using Domain.Constants;
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
        public int? BusinessPartnerId { get; set; }
        public DocType PaymentFormType { get; set; }
        [Required]
        public string? AccountId { get; set; }
        [Required]
        public DateTime? PaymentDate { get; set; }
        [Required]
        public PaymentRegisterType? PaymentRegisterType { get; set; }
        [Required]
        public string? PaymentRegisterId { get; set; }
        [Required]
        [MaxLength(500)]
        public string Description { get; set; }
        public int? CampusId { get; set; }
        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Gross amount must be greater than 0")]
        public decimal? GrossPayment { get; set; }
        [Range(0.00, 100.00, ErrorMessage = "Please enter a value between 0 and 100")]
        public decimal? SalesTax { get; set; }
        [Range(0.00, 100.00, ErrorMessage = "Please enter a value between 0 and 100")]
        public decimal? IncomeTax { get; set; }
        [Range(0.00, 100.00, ErrorMessage = "Please enter a value between 0 and 100")]
        public decimal? SRBTax { get;  set; }
        [MaxLength(20)]
        public string ChequeNo { get; set; }
        public decimal? Deduction { get; set; }
        public int? DocumentLedgerId { get; set; }
        public string? DeductionAccountId { get; set; }
        [Required]
        public bool? isSubmit { get; set; }
    }
}
