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
    public class IssuanceToIssuanceReturnLineReconcileRepository : IIssuanceToIssuanceReturnLineReconcileRepository
    {
        private readonly ApplicationDbContext _context;

        public IssuanceToIssuanceReturnLineReconcileRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IssuanceToIssuanceReturnLineReconcile> Add(IssuanceToIssuanceReturnLineReconcile entity)
        {
            var result = await _context.IssuanceToIssuanceReturnLineReconcile.AddAsync(entity);
            return result.Entity;
        }

        public IEnumerable<IssuanceToIssuanceReturnLineReconcile> Find(ISpecification<IssuanceToIssuanceReturnLineReconcile> specification)
        {
            return SpecificationEvaluator<IssuanceToIssuanceReturnLineReconcile, int>.GetQuery(_context.IssuanceToIssuanceReturnLineReconcile
                                     .AsQueryable(), specification);
        }
    }
}
