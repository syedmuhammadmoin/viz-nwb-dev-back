using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs.Quotation
{
    public class GetQouteByReqDto
    {
        public int RequisitionId { get; set; }
        public int? QuotationCompId { get; set; }
    }
}
