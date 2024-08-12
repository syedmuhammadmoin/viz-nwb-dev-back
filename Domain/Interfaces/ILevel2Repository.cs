using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ILevel2Repository : IGenericRepository<Level2, string>
    {
        Task AddRange(List<Level2> list);
    }
}
