using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreateQuotationComparativeLinesDto
    {
        public int QuotationId { get; set; }
        public bool isSelected { get; set; }
    }
}
