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
    public class WorkFlowTransitionRepository : IWorkFlowTransitionRepository
    {

        private readonly ApplicationDbContext _context;

        public WorkFlowTransitionRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IEnumerable<WorkFlowTransition> Find(ISpecification<WorkFlowTransition> specification)
        {
            return SpecificationEvaluator<WorkFlowTransition, int>.GetQuery(_context.WorkFlowTransitions
                                   .AsQueryable(), specification);
        }
    }
}
