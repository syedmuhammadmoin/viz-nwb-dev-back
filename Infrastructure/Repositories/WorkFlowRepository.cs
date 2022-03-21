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
    public class WorkFlowRepository : GenericRepository<WorkFlowMaster, int>, IWorkFlowRepository
    {
        private readonly ApplicationDbContext _context;
        public WorkFlowRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<WorkFlowMaster> Find(ISpecification<WorkFlowMaster> specification)
        {
            return await SpecificationEvaluator<WorkFlowMaster, int>.GetQuery(_context.WorkFlowMaster
                                    .AsQueryable(), specification)
                                    .FirstOrDefaultAsync();
        }
    }

    public class WorkFlowStatusRepository : GenericRepository<WorkFlowStatus, int>, IWorkFlowStatusRepository
    {
        public WorkFlowStatusRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
