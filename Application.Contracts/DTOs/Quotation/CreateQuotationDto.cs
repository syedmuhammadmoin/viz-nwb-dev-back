
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreateQuotationDto
    {
        public int? Id { get;  set; }
        [Required]
        public int VendorId { get; set; }
        [Required]
        public DateTime QuotationDate { get; set; }
        [MaxLength(100)]
        public string Timeframe { get;  set; }
        [Required]
        public bool? isSubmit { get; set; }
        public int RequisitionId { get; set; }
        public int CallForQuotationId { get; set; }
        public virtual List<CreateQuotationLinesDto> QuotationLines { get;  set; }
    }
}
