using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs.QuotationComparative
{
    public class CreateQuotationComparativeLinesDto
    {
        public int QuotationId { get; set; }
        public bool isRemove { get; set; }
    }
}
