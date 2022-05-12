using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IPayrollItemEmpRepository
    {
        Task AddRange(List<PayrollItemEmployee> list);
        IEnumerable<PayrollItemEmployee> Find(ISpecification<PayrollItemEmployee> specification);
        Task<bool> RemoveAllByPayrollItemId(int id);
    }
}
