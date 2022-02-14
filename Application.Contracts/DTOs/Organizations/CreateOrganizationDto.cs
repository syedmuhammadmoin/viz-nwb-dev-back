using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreateOrganizationDto
    {
        public int? Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
        [MaxLength(50)]
        public string Country { get; set; }
        [MaxLength(50)]
        public string State { get; set; }
        [MaxLength(50)]
        public string City { get; set; }
        [MaxLength(200)]
        public string Address { get; set; }
        [MaxLength(20)]
        public string Phone { get; set; }
        [MaxLength(20)]
        public string Fax { get; set; }
        [MaxLength(20)]
        public string Email { get; set; }
        [MaxLength(50)]
        public string Website { get; set; }
        //Industry will link from chart of account in future...
        [MaxLength(50)]
        public string Industry { get; set; }
        //this will link from tax in future
        [MaxLength(50)]
        public string LegalStatus { get; set; }
        [MaxLength(50)]
        public string IncomeTaxId { get; set; }
        [MaxLength(50)]
        public string SalesTaxId { get; set; }
        [Required]

        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public int ClientId { get; set; }
    }
}
