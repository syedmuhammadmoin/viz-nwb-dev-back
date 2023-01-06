using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class AwardedVendorDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Remarks { get; set; }
        [Required]
        public int QuotationId { get; set; }
     
    }
}
