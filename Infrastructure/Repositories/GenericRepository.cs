using Domain.Base;
using Domain.Interfaces;
using Infrastructure.Context;
using Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class GenericRepository<T, TKey> : IGenericRepository<T, TKey> where T : BaseEntity<TKey>
    {
        private readonly ApplicationDbContext _context;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<T> Add(T entity)
        {
            var result = await _context.Set<T>().AddAsync(entity);
            return result.Entity;
        }

        public async Task<IEnumerable<T>> GetAll(ISpecification<T> specification = null)
        {
            return await SpecificationEvaluator<T, TKey>.GetQuery(_context.Set<T>()
                                    .AsQueryable(), specification)
                                    .AsNoTracking()
                                    .ToListAsync();
        }

        public Task<bool> Delete(TKey id)
        {
            throw new NotImplementedException();
        }

        public async Task<T> GetById(TKey id, ISpecification<T> specification = null)
        {

            var query = SpecificationEvaluator<T, TKey>.GetQuery(_context.Set<T>()
                            .Where(x => x.Id.Equals(id))
                            .AsQueryable(), specification);

            var rawSql = query.ToQueryString();

            return await query.FirstOrDefaultAsync();

        }

        public async Task<int> TotalRecord(ISpecification<T> specification = null)
        {
            return await SpecificationEvaluator<T, TKey>.GetQuery(_context.Set<T>()
                                    .AsQueryable(), specification)
                                    .AsNoTracking()
                                    .CountAsync();
        }

        public IEnumerable<T> Find(ISpecification<T> specification)
        {
            return SpecificationEvaluator<T, TKey>.GetQuery(_context.Set<T>()
                                    .AsQueryable(), specification);
        }
        public async Task<bool> Any(ISpecification<T> specification = null)
        {
            return await SpecificationEvaluator<T, TKey>.GetQuery(_context.Set<T>()
                                    .AsQueryable(), specification)
                                    .AnyAsync();
        }
    }
}
