using Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs.Tax
{
    public class TaxInvoiceLinesDto
    {
        public int Id { get; set; }
        public TaxBase TaxBase { get; set; }
        public Guid AccountId { get; set; }
        public int MasterId { get; set; }
    }
}
