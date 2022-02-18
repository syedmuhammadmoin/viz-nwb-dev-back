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
    public class BusinessPartnerService : IBusinessPartnerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BusinessPartnerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Response<BusinessPartnerDto>> CreateAsync(CreateBusinessPartnerDto entity)
        {
            var businessPartner = _mapper.Map<BusinessPartner>(entity);
            var result = await _unitOfWork.BusinessPartner.Add(businessPartner);
            await _unitOfWork.SaveAsync();

            return new Response<BusinessPartnerDto>(_mapper.Map<BusinessPartnerDto>(result), "Created successfully");
        }

        public async Task<PaginationResponse<List<BusinessPartnerDto>>> GetAllAsync(PaginationFilter filter)
        {
            var specification = new BusinessPartnerSpecs(filter);
            var businessPartner = await _unitOfWork.BusinessPartner.GetAll(specification);

            if (businessPartner.Count() == 0)
                return new PaginationResponse<List<BusinessPartnerDto>>("List is empty");

            var totalRecords = await _unitOfWork.BusinessPartner.TotalRecord();

            return new PaginationResponse<List<BusinessPartnerDto>>(_mapper.Map<List<BusinessPartnerDto>>(businessPartner), filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<BusinessPartnerDto>> GetByIdAsync(int id)
        {
            var businessPartner = await _unitOfWork.BusinessPartner.GetById(id);
            if (businessPartner == null)
                return new Response<BusinessPartnerDto>("Not found");

            return new Response<BusinessPartnerDto>(_mapper.Map<BusinessPartnerDto>(businessPartner), "Returning value");
        }

        public async Task<Response<BusinessPartnerDto>> UpdateAsync(CreateBusinessPartnerDto entity)
        {
            var businessPartner = await _unitOfWork.BusinessPartner.GetById((int)entity.Id);

            if (businessPartner == null)
                return new Response<BusinessPartnerDto>("Not found");

            //For updating data
            _mapper.Map<CreateBusinessPartnerDto, BusinessPartner>(entity, businessPartner);
            await _unitOfWork.SaveAsync();
            return new Response<BusinessPartnerDto>(_mapper.Map<BusinessPartnerDto>(businessPartner), "Updated successfully");
        }
        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
