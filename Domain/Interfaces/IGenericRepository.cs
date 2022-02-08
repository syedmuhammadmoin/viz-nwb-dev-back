using Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IGenericRepository<T, TKey> where T : BaseEntity<TKey>
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(TKey id);
        Task<T> Add(T entity);
        Task<bool> Delete(TKey id);
        IEnumerable<T> Find(ISpecification<T> specification = null);
    }
}
