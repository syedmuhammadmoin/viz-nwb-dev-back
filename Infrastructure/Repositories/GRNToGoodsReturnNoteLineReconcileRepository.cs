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
    public class GRNToGoodsReturnNoteLineReconcileRepository : IGRNToGoodsReturnNoteLineReconcileRepository
    {
        private readonly ApplicationDbContext _context;

        public GRNToGoodsReturnNoteLineReconcileRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<GRNToGoodsReturnNoteLineReconcile> Add(GRNToGoodsReturnNoteLineReconcile entity)
        {
            var result = await _context.GRNToGoodsReturnNoteLineReconcile.AddAsync(entity);
            return result.Entity;
        }
        public IEnumerable<GRNToGoodsReturnNoteLineReconcile> Find(ISpecification<GRNToGoodsReturnNoteLineReconcile> specification)
        {
            return SpecificationEvaluator<GRNToGoodsReturnNoteLineReconcile, int>.GetQuery(_context.GRNToGoodsReturnNoteLineReconcile
                                    .AsQueryable(), specification);
        }
    }
}
