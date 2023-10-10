using Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class PayrollCampusTransactionDto
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public int CampusId { get; set; }
        public string Campus { get; set; }
        public decimal AdvancesAndDeductions { get; set; }
        public decimal IncomeTax{ get; set; }
        public decimal NetAmount{ get; set; }
        public decimal GrossSalary { get; set; }
        public string Status { get; set; }
        public DocumentStatus State { get; set; }

    }
}
