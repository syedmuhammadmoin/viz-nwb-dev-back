using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Interfaces
{
    public interface IWorkFlowService : ICrudService<CreateWorkFlowDto, WorkFlowDto, int, PaginationFilter>
    {
    }

    public interface IWorkFlowStatusService : ICrudService<CreateWorkFlowStatusDto, WorkFlowStatusDto, int, PaginationFilter>
    {
    }
}
