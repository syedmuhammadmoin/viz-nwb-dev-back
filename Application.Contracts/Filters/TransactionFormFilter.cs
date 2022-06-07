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
        public string Warehouse { get; set; }
        public string Name { get; set; }
        public string Account { get; set; }
        public string Category { get; set; }
        public DateTime? DocDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DocumentStatus? State { get; set; }
        public bool? IsActive { get; set; }
    }
}
