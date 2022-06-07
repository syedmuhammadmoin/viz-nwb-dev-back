using Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Filters
{
    public class TransactionFormFilter : PaginationFilter
    {
        public string DocNo { get; set; }
        public string BusinessPartner { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public DateTime? DocDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DocumentStatus? State { get; set; }
        public decimal? Amount { get; set; }
        public bool? isActive { get; set; }


    }
}
