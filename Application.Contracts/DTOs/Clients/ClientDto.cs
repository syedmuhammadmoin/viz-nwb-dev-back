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
        public string Name { get; private set; }
        public string Country { get; private set; }
        public string State { get; private set; }
        public string City { get; private set; }
        public string Address { get; private set; }
        public string Phone { get; private set; }
        public string Fax { get; private set; }
        public string Email { get; private set; }
        public string Website { get; private set; }
        public string BankName { get; private set; }
        public string BankAccountTitle { get; private set; }
        public string BankAccountNumber { get; private set; }
        public string Currency { get; private set; }
    }
}
