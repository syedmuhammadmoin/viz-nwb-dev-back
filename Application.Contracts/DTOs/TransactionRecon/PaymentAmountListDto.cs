using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class PaymentAmountListDto
    {
        public int LedgerId { get; set; }
        public decimal UnreconciledAmount { get; set; }
    }
}
