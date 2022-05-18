using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class PayrollItemEmployeeDto
    {
        public int PayrollItemId { get; set; }
        public int EmployeeId { get; set; }
        public virtual List<EmployeeDto> Employee { get; set; }
    }
}
