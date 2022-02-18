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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrganizationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Response<OrganizationDto>> CreateAsync(CreateOrganizationDto entity)
        {
            var org = _mapper.Map<Organization>(entity);
            var result = await _unitOfWork.Organization.Add(org);
            await _unitOfWork.SaveAsync();

            return new Response<OrganizationDto>(_mapper.Map<OrganizationDto>(result), "Created successfully");
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PaginationResponse<List<OrganizationDto>>> GetAllAsync(PaginationFilter filter)
        {
            var specification = new OrganizationSpecs(filter);
            var orgs = await _unitOfWork.Organization.GetAll(specification);

            if (orgs.Count() == 0)
                return new PaginationResponse<List<OrganizationDto>>("List is empty");

            var totalRecords = await _unitOfWork.Organization.TotalRecord();

            return new PaginationResponse<List<OrganizationDto>>(_mapper.Map<List<OrganizationDto>>(orgs),
                filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<OrganizationDto>> GetByIdAsync(int id)
        {
            var org = await _unitOfWork.Organization.GetById(id);
            if (org == null)
                return new Response<OrganizationDto>("Not found");

            return new Response<OrganizationDto>(_mapper.Map<OrganizationDto>(org), "Returning value");
        }

        public async Task<Response<OrganizationDto>> UpdateAsync(CreateOrganizationDto entity)
        {
            var org = await _unitOfWork.Organization.GetById((int)entity.Id);

            if (org == null)
                return new Response<OrganizationDto>("Not found");

            //For updating data
            _mapper.Map<CreateOrganizationDto, Organization>(entity, org);
            await _unitOfWork.SaveAsync();
            return new Response<OrganizationDto>(_mapper.Map<OrganizationDto>(org), "Updated successfully");
        }
    }
}
