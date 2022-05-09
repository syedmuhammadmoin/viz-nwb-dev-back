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
    public interface IDesignationService : ICrudService<DesignationDto, DesignationDto, int, PaginationFilter>
    {
        Task<Response<List<DesignationDto>>> GetDesignationDropDown();
    }
}