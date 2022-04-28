﻿using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Interfaces
{
    public interface IRequisitionService : ICrudService<CreateRequisitionDto, RequisitionDto, int, PaginationFilter>
    {
        Task<Response<bool>> CheckWorkFlow(ApprovalDto data);
    }
}
