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
    public class RemarkRepository : IRemarkRepository
    {
        private readonly ApplicationDbContext _context;
        public RemarkRepository(ApplicationDbContext context)
        {
            _context = context;

        }
        public async Task<Remark> Add(Remark entity)
        {
            var result = await _context.Remarks.AddAsync(entity);
            return result.Entity;
        }

        public IEnumerable<Remark> Find(ISpecification<Remark> specification)
        {
            return SpecificationEvaluator<Remark, int>.GetQuery(_context.Remarks
                                   .AsQueryable(), specification);
        }
    }
}
