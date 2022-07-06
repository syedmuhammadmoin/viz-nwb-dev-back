using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;
using Infrastructure.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class IssuanceToGRNLineReconcileRepository : IIssuanceToGRNLineReconcileRepository
    {
        private readonly ApplicationDbContext _context;

        public IssuanceToGRNLineReconcileRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IssuanceToGRNLineReconcile> Add(IssuanceToGRNLineReconcile entity)
        {
            var result = await _context.IssuanceToGRNLineReconcile.AddAsync(entity);
            return result.Entity;
        }

        public IEnumerable<IssuanceToGRNLineReconcile> Find(ISpecification<IssuanceToGRNLineReconcile> specification)
        {
            return SpecificationEvaluator<IssuanceToGRNLineReconcile, int>.GetQuery(_context.IssuanceToGRNLineReconcile
                                     .AsQueryable(), specification);
        }
    }
}
