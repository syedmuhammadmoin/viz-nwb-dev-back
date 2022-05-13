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
    public class BankStmtLinesRepository : IBankStmtLinesRepository
    {
        private readonly ApplicationDbContext _context;

        public BankStmtLinesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<BankStmtLines> Find(ISpecification<BankStmtLines> specification)
        {
            return SpecificationEvaluator<BankStmtLines, int>.GetQuery(_context.BankStmtLines
                                       .AsQueryable(), specification);
        }

        public async Task<IEnumerable<BankStmtLines>> GetAll(ISpecification<BankStmtLines> specification = null)
        {
            return await SpecificationEvaluator<BankStmtLines, int>.GetQuery(_context.BankStmtLines
                                   .AsQueryable(), specification)
                                   .AsNoTracking()
                                   .ToListAsync();
        }

        public async Task<BankStmtLines> GetById(int id, ISpecification<BankStmtLines> specification = null)
        {
            return await SpecificationEvaluator<BankStmtLines, int>.GetQuery(_context.BankStmtLines
                                     .Where(x => x.Id.Equals(id))
                                     .AsQueryable(), specification)
                                     .FirstOrDefaultAsync();
        }
    }
}
