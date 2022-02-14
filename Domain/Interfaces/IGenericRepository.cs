﻿using Domain.Base;
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
        Task<IEnumerable<T>> GetAll(ISpecification<T> specification = null);
        Task<T> GetById(TKey id, ISpecification<T> specification = null);
        Task<T> Add(T entity);
        Task<bool> Delete(TKey id);
        Task<int> TotalRecord();
        IEnumerable<T> Find(ISpecification<T> specification = null);
    }
}
