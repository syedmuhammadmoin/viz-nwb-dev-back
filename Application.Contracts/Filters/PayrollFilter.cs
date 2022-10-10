using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Filters
{
    public class PayrollFilter
    {
        public int? EmployeeId { get; set; }
        //public string EmployeeName { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public string Campus { get; set; }
        public string BPS { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
