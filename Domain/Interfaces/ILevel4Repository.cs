using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ILevel4Repository : IGenericRepository<Level4, string>
    {
        Task AddRange(List<Level4> list);

        Task<List<Level1>> GetCOA();
        IQueryable<Level1> GetAccoutTypes();
    }
}
