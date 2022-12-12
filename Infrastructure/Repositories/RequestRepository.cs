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
    public class RequestRepository : GenericRepository<RequestMaster, int>, IRequestRepository
    {
        private readonly ApplicationDbContext _context;
        public RequestRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<RequestLines> FindLines(ISpecification<RequestLines> specification)
        {
            return SpecificationEvaluator<RequestLines, int>.GetQuery(_context.RequestLines
                .AsQueryable(), specification);
        }
    }
}
