using System.ComponentModel.DataAnnotations;

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
        [MaxLength(50)]
        public string Street { get; private set; }
        [MaxLength(50)]
        public string Block { get; private set; }
        [MaxLength(50)]
        public string Road { get; private set; }
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
