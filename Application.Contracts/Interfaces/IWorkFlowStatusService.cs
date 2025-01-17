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

    public interface IWorkFlowStatusService : ICrudService<CreateWorkFlowStatusDto, WorkFlowStatusDto, int, TransactionFormFilter>
    {
        Task<Response<List<WorkFlowStatusDto>>> GetStatusDropDown();
    }
}
