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
        public decimal TenureAmount { get; set; }
        public decimal ContractualAmount { get; set; }
        public decimal RegularAmount { get; set; }
        public virtual List<PayrollItemsDto> PayrollItems { get; set; }
    }
    public class PayrollItemsDto
    {
        public string AccountId { get; set; }
        public string AccountName { get; set; }
        public PayrollType PayrollType { get; set; }
        public decimal Amount { get; set; }
    }
}
