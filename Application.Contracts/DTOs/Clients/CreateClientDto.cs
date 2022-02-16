using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreateClientDto
    {
        public int? Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(50)]
        public string Country { get; set; }
        [MaxLength(50)]
        public string State { get; set; }
        [MaxLength(50)]
        public string City { get; set; }
        [MaxLength(20)]
        public string Address { get; set; }
        [MaxLength(20)]
        public string Phone { get; set; }
        [MaxLength(20)]
        public string Fax { get; set; }
        [MaxLength(20)]
        public string Email { get; set; }
        [MaxLength(50)]
        public string Website { get; set; }
        [Required]
        [MaxLength(50)]
        public string BankName { get; set; }
        [Required]
        [MaxLength(50)]
        public string BankAccountTitle { get; set; }
        [Required]
        [MaxLength(30)]
        public string BankAccountNumber { get; set; }
        [Required]
        [MaxLength(30)]
        public string Currency { get; set; }
    }
}
