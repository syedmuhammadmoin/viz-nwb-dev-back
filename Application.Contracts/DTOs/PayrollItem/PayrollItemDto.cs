using Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class PayrollItemDto
    {
        public int Id { get; set; }
        public string ItemCode { get; set; }
        public string Name { get; set; }
        public PayrollType PayrollType { get; set; }
        public CalculationType PayrollItemType { get; set; }
        public decimal Value { get; set; }
        public Guid AccountId { get; set; }
        public string AccountName { get; set; }
        public bool IsActive { get; set; }
        public string Remarks { get; set; }
        public List<EmployeeDto> Employees { get; set; }
    }
}
