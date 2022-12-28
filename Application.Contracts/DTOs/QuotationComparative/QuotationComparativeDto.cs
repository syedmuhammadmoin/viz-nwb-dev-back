using Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class QuotationComparativeDto
    {
        public int Id { get; set; }
        public string DocNo { get; set; }
        public int RequisitionId { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }
        public DocumentStatus State { get; set; }
        public virtual List<QuotationDto> Quotations { get; set; }
  

    }
}
