using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IBankStmtLinesRepository
    {
        Task<IEnumerable<BankStmtLines>> GetAll(ISpecification<BankStmtLines> specification = null);
        Task<BankStmtLines> GetById(int id, ISpecification<BankStmtLines> specification = null);
        IEnumerable<BankStmtLines> Find(ISpecification<BankStmtLines> specification);
    }
}
