using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class ClientDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string BankName { get; set; }
        public string BankAccountTitle { get; set; }
        public string BankAccountNumber { get; set; }
        public string Currency { get; set; }
    }
}
