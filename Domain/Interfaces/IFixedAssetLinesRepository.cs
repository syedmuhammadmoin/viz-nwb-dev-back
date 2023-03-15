using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IFixedAssetLinesRepository : IGenericRepository<FixedAssetLines, int>
    {
        Task<IEnumerable<FixedAssetLines>> GetByMonthAndYear(int fixedAssetId, int month, int year);
    }
}
