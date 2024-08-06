using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ILevel3Repository : IGenericRepository<Level3, Guid>
    {
        Task AddRange(List<Level3> list);
    }
}
