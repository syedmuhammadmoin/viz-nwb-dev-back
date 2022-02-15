using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class BusinessPartnerDto
    {
        public int Id { get; set; }
        public string BusinessPartnerType { get; set; }
        public string Entity { get; set; }
        public string Name { get; set; }
        public string CNIC { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string IncomeTaxId { get; set; }
        public string SalesTaxId { get; set; }
        public string BankAccountTitle { get; set; }
        public string BankAccountNumber { get; set; }
        public Guid AccountReceivableId { get; set; }
        public string AccountReceivable { get; set; }
        public Guid AccountPayableId { get; set; }
        public string AccountPayable { get; set; }
    }
}
