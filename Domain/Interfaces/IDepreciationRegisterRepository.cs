using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IDepreciationRegisterRepository : IGenericRepository<DepreciationRegister, int>
    {
        Task<IEnumerable<DepreciationRegister>> GetByMonthAndYear(int fixedAssetId, int month, int year);
    }
}
