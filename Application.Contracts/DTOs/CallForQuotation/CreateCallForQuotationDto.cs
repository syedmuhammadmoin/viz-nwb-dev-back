using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreateCallForQuotationDto
    {
        public int? Id { get; set; }
        [Required]
        public int? VendorId { get; set; }
        [Required]
        public DateTime? CallForQuotationDate { get; set; }
        [MaxLength(100)]
        public string Description { get; set; }
        [Required]
        public bool? isSubmit { get; set; }
        [Required]
        public virtual List<CreateCallForQuotationLinesDto> CallForQuotationLines { get; set; }
    }
}
