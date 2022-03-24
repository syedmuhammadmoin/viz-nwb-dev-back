using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;
using Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class TransactionReconcileRepository : ITransactionReconcileRepository
    {
        private readonly ApplicationDbContext _context;

        public TransactionReconcileRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<TransactionReconcile> Reconcile(TransactionReconcile entity)
        {
            var result = await _context.TransactionReconciles.AddAsync(entity);
            return result.Entity;
        }
        public IEnumerable<TransactionReconcile> Find(ISpecification<TransactionReconcile> specification)
        {
            return SpecificationEvaluator<TransactionReconcile, int>.GetQuery(_context.TransactionReconciles
                                    .AsQueryable(), specification);
        }
    }
}
