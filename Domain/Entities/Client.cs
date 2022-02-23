using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Client : BaseEntity<int>
    {
        [MaxLength(100)]
        public string Name { get; private set; }
        [MaxLength(50)]
        public string Country { get; private set; }
        [MaxLength(50)]
        public string State { get; private set; }
        [MaxLength(50)]
        public string City { get; private set; }
        [MaxLength(200)]
        public string Address { get; private set; }
        [MaxLength(20)]
        public string Phone { get; private set; }
        [MaxLength(20)]
        public string Fax { get; private set; }
        [MaxLength(20)]
        public string Email { get; private set; }
        [MaxLength(50)]
        public string Website { get; private set; }
        [MaxLength(50)]
        public string BankName { get; private set; }
        [MaxLength(50)]
        public string BankAccountTitle { get; private set; }
        [MaxLength(30)]
        public string BankAccountNumber { get; private set; }
        [MaxLength(30)]
        public string Currency { get; private set; }
        public List<Organization> Organizations { get; set; }

        protected Client()
        {

        }
    }
}
