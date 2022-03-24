using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class AmountsForReconciliationDto
    {
        public decimal TotalAmount { get; set; }
        public decimal ReconciledAmount { get; set; }
        public decimal UnreconciledAmount { get; set; }
    }
}
