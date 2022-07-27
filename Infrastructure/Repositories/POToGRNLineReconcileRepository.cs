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
    public class POToGRNLineReconcileRepository : IPOToGRNLineReconcileRepository
    {
        private readonly ApplicationDbContext _context;

        public POToGRNLineReconcileRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        
        public async Task<POToGRNLineReconcile> Add(POToGRNLineReconcile entity)
        {
            var result = await _context.POToGRNLineReconcile.AddAsync(entity);
            return result.Entity;
        }
        public IEnumerable<POToGRNLineReconcile> Find(ISpecification<POToGRNLineReconcile> specification)
        {
            return SpecificationEvaluator<POToGRNLineReconcile, int>.GetQuery(_context.POToGRNLineReconcile
                                    .AsQueryable(), specification);
        }
    }
}
