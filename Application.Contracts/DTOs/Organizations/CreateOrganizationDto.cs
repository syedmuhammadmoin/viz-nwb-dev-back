﻿using System.ComponentModel.DataAnnotations;

namespace Application.Contracts.DTOs
{
    public class CreateOrganizationDto
    {
        public int? Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(50)]
        public string Country { get; set; }
        [Required]
        [MaxLength(50)]
        public string State { get; set; }
        [Required]
        [MaxLength(50)]
        public string City { get; set; }
        [MaxLength(200)]
        public string Address { get; set; }
        [MaxLength(20)]
        public string Phone { get; set; }
        [MaxLength(20)]
        public string Fax { get; set; }
        [MaxLength(50)]
        public string Email { get; set; }
        [MaxLength(100)]
        public string Website { get; set; }
        //Industry will link from chart of account in future...
        [MaxLength(100)]
        public string Industry { get; set; }
        //this will link from tax in future
        [MaxLength(100)]
        public string LegalStatus { get; set; }
        [MaxLength(100)]
        public string IncomeTaxId { get; set; }
        [MaxLength(100)]
        public string GSTRegistrationNo { get; set; }
        [Required]
        public DateTime? FiscalYearStart { get; set; }
        [Required]
        public DateTime? FiscalYearEnd { get; set; }
        public string UserId { get; set; }
    }
}
