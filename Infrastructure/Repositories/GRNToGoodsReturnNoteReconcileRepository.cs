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
    public class GRNToGoodsReturnNoteReconcileRepository : IGRNToGoodsReturnNoteReconcileRepository
    {
        private readonly ApplicationDbContext _context;

        public GRNToGoodsReturnNoteReconcileRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<GRNToGoodsReturnNoteReconcile> Add(GRNToGoodsReturnNoteReconcile entity)
        {
            var result = await _context.GRNToGoodsReturnNoteReconcile.AddAsync(entity);
            return result.Entity;
        }
        public IEnumerable<GRNToGoodsReturnNoteReconcile> Find(ISpecification<GRNToGoodsReturnNoteReconcile> specification)
        {
            return SpecificationEvaluator<GRNToGoodsReturnNoteReconcile, int>.GetQuery(_context.GRNToGoodsReturnNoteReconcile
                                    .AsQueryable(), specification);
        }
    }
}
