﻿using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Response;

namespace Application.Contracts.Interfaces
{
    public interface ISubjectService : ICrudService<SubjectDto, SubjectDto, int, TransactionFormFilter>
    {
        Task<Response<List<SubjectDto>>> GetDropDown();
    }
}
