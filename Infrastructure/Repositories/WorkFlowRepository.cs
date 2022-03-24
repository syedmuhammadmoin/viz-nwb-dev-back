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
        public WorkFlowRepository(ApplicationDbContext context) : base(context)
        {
        }
    }

    public class WorkFlowStatusRepository : GenericRepository<WorkFlowStatus, int>, IWorkFlowStatusRepository
    {
        public WorkFlowStatusRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
