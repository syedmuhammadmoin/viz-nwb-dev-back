using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreateCampusDto
    {
        public int? Id { get; set; }
        [MaxLength(100)]
        [Required]
        public string Name { get; set; }
        [MaxLength(300)]
        [Required]
        public string Address { get; set; }
        [MaxLength(20)]
        public string Contact { get; set; }
        [MaxLength(20)]
        public string Fax { get; set; }
        [MaxLength(500)]
        public string Email { get; set; }
        [MaxLength(500)]
        public string SalesTaxId { get; set; }
        [MaxLength(50)]
        public string NTN { get; set; }
        [MaxLength(50)]
        public string SRB { get; set; }
    }
}
