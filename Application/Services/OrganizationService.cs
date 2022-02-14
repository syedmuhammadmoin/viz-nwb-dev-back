using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class OrganizationService : IOrganizationService
    {
        public Task<Response<OrganizationDto>> CreateAsync(CreateOrganizationDto entity)
        {
            throw new NotImplementedException();
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PaginationResponse<List<OrganizationDto>>> GetAllAsync(PaginationFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task<Response<OrganizationDto>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Response<OrganizationDto>> UpdateAsync(CreateOrganizationDto entity)
        {
            throw new NotImplementedException();
        }
    }
}
