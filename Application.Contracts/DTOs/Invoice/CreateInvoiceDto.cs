using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreateInvoiceDto
    {
        public int? Id { get; set; }
        [Required]
        public int? CustomerId { get; set; }
        [Required]
        public DateTime? InvoiceDate { get; set; }
        [Required]
        public DateTime? DueDate { get; set; }
        [MaxLength(20)]
        public string Contact { get; set; }       
        public int? CampusId { get; set; }
        [Required]
        public bool? isSubmit { get; set; }
        [Required]
        public virtual List<CreateInvoiceLinesDto> InvoiceLines { get; set; }
    }
}
