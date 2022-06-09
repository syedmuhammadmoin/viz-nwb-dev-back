using Domain.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreateBusinessPartnerDto
    {
        public int? Id { get; set; }
        [Required]
        public BusinessPartnerType BusinessPartnerType { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string CNIC { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string IncomeTaxId { get; set; }
        public string SalesTaxId { get; set; }
        public string BankAccountTitle { get; set; }
        public string BankAccountNumber { get; set; }
        [Required]
        public Guid AccountReceivableId { get; set; }
        [Required]
        public Guid AccountPayableId { get; set; }
    }
}
