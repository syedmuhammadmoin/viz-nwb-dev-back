using Application.Contracts.DTOs;
using Application.Contracts.DTOs.Organizations;
using Application.Contracts.Filters;
using Application.Contracts.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Interfaces
{
    public interface IOrganizationService : ICrudService<CreateOrganizationDto, UpdateOrganizationDto, OrganizationDto, int, TransactionFormFilter>
    {
        Task<Response<List<OrganizationDto>>> GetOrganizationDropDown();
        Task<Response<List<UsersOrganizationDto>>> GetOrganizationByUserId();
        Task<Response<OrganizationDto>> CreateAsync(CreateOrganizationDto entity, string token);
        Task<Response<OrganizationDto>> Create2Async(CreateOrganizationDto entity);
    }
}
