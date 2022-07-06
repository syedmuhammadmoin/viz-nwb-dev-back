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
    public class IssuanceRepository : GenericRepository<IssuanceMaster, int>, IIssuanceRepository
    {
        private readonly ApplicationDbContext _context;
        public IssuanceRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<IssuanceLines> FindLines(ISpecification<IssuanceLines> specification)
        {
            return SpecificationEvaluator<IssuanceLines, int>.GetQuery(_context.IssuanceLines
                                    .AsQueryable(), specification);
        }
    }
}
