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
        public BusinessPartnerType? BusinessPartnerType { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(20)]
        public string CNIC { get; set; }
        [MaxLength(200)]
        public string Address { get; set; }
        [MaxLength(20)]
        public string Phone { get; set; }
        [MaxLength(20)]
        public string Mobile { get; set; }
        [MaxLength(50)]
        public string IncomeTaxId { get; set; }
        [MaxLength(50)]
        public string SalesTaxId { get; set; }
        [MaxLength(50)]
        public string BankAccountTitle { get; set; }
        [MaxLength(30)]
        public string BankAccountNumber { get; set; }
        [Required]
        public Guid? AccountReceivableId { get; set; }
        [Required]
        public Guid? AccountPayableId { get; set; }
    }
}
