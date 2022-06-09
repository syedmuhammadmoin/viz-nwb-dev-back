using Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class AmountsForReconciliationDto
    {
        public int DocumentId { get; set; }
        public int PaymentLedgerId { get; set; }
        public string DocNo { get; set; }
        public DocType DocType { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal ReconciledAmount { get; set; }
        public decimal UnreconciledAmount { get; set; }
    }
}
