using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IPayrollItemRepository : IGenericRepository<PayrollItem, int>
    {
        public List<PayrollResult> GetPayrollItemsByEmployeeId(int id);
    }
}
