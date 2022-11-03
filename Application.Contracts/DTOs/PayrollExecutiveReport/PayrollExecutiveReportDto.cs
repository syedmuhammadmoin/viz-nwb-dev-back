using Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class PayrollExecutiveReportDto
    {
        public string PayrollItem { get; set; }
        public PayrollType PayrollType { get; set; }
        public decimal Amount { get; set; }
    }
}
