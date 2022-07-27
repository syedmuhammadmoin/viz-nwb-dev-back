using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IRemarkRepository
    {
        IEnumerable<Remark> Find(ISpecification<Remark> specification);
        Task<Remark> Add(Remark entity);
    }
}
