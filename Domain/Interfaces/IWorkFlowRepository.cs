using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IWorkFlowRepository : IGenericRepository<WorkFlowMaster, int>
    {
        Task<WorkFlowMaster> Find(ISpecification<WorkFlowMaster> specification);
    }

    public interface IWorkFlowStatusRepository : IGenericRepository<WorkFlowStatus, int>
    {

    }
}
