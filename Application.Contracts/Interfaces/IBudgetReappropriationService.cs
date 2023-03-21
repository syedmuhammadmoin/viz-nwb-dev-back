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
    public interface IBudgetReappropriationService : ICrudService<CreateBudgetReappropriationDto, BudgetReappropriationDto, int, TransactionFormFilter>
    {
        Task<Response<bool>> CheckWorkFlow(ApprovalDto data);
        Task<Response<List<BudgetReappropriationDto>>> GetDropDown();
    }
}