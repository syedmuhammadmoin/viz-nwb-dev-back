using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{

    public class DepreciationAdjustmentRepository : GenericRepository<DepreciationAdjustmentMaster, int>, IDepreciationAdjustmentRepository
    {
        public DepreciationAdjustmentRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
