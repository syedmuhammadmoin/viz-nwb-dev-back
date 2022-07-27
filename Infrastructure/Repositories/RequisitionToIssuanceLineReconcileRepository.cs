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
    public class RequisitionToIssuanceLineReconcileRepository : IRequisitionToIssuanceLineReconcileRepository
    {
        private readonly ApplicationDbContext _context;

        public RequisitionToIssuanceLineReconcileRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RequisitionToIssuanceLineReconcile> Add(RequisitionToIssuanceLineReconcile entity)
        {
            var result = await _context.RequisitionToIssuanceLineReconcile.AddAsync(entity);
            return result.Entity;
        }

        public IEnumerable<RequisitionToIssuanceLineReconcile> Find(ISpecification<RequisitionToIssuanceLineReconcile> specification)
        {
            return SpecificationEvaluator<RequisitionToIssuanceLineReconcile, int>.GetQuery(_context.RequisitionToIssuanceLineReconcile
                                    .AsQueryable(), specification);
        }
    }
}
