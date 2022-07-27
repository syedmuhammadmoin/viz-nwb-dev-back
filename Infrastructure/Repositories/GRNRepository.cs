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
    public class GRNRepository : GenericRepository<GRNMaster, int>, IGRNRepository
    {
        private readonly ApplicationDbContext _context;
        public GRNRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<GRNLines> FindLines(ISpecification<GRNLines> specification)
        {
            return SpecificationEvaluator<GRNLines, int>.GetQuery(_context.GRNLines
                                      .AsQueryable(), specification);
        }
    }
}
