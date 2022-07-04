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
    public class RequisitionRepository : GenericRepository<RequisitionMaster, int>, IRequisitionRepository
    {
        private readonly ApplicationDbContext _context;
        public RequisitionRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<RequisitionLines> FindLines(ISpecification<RequisitionLines> specification)
        {
            return SpecificationEvaluator<RequisitionLines, int>.GetQuery(_context.RequisitionLines
                                    .AsQueryable(), specification);
        }
    }
}
