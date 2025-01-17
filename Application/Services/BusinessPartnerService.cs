﻿using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Helper;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using AutoMapper;
using Domain.Constants;
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

            var Receivablelevel4 = await _unitOfWork.Level4.GetById(entity.AccountReceivableId);
            //SBBU-Code
            //var AccountReceivable = ReceivableAndPayable.ValidateReceivable((Guid)Receivablelevel4.Level3_id);

            ////Validation for Receivable
            //if (AccountReceivable == false)
            //{
            //    return new Response<BusinessPartnerDto>("Account Receivable is Invalid");
            //}

            var Payablelevel4 = await _unitOfWork.Level4.GetById(entity.AccountPayableId);
            //SBBU-Code
            //var AccountPayable = ReceivableAndPayable.ValidatePayable((Guid)Payablelevel4.Level3_id);

            ////Validation for Payable
            //if (AccountPayable == false)
            //{
            //    return new Response<BusinessPartnerDto>("Account Payable is Invalid");
            //}
            var result = await _unitOfWork.BusinessPartner.Add(businessPartner);
            await _unitOfWork.SaveAsync();

            return new Response<BusinessPartnerDto>(_mapper.Map<BusinessPartnerDto>(result), "Created successfully");
        }

        public async Task<PaginationResponse<List<BusinessPartnerDto>>> GetAllAsync(BusinessPartnerFilter filter)
        {
            var businessPartnerTypes = new List<BusinessPartnerType?>();
            if (filter.Type != null)
            {
                businessPartnerTypes.Add(filter.Type);
            }

            var businessPartner = await _unitOfWork.BusinessPartner.GetAll(new BusinessPartnerSpecs(businessPartnerTypes, filter, false));

            if (businessPartner.Count() == 0)
                return new PaginationResponse<List<BusinessPartnerDto>>(_mapper.Map<List<BusinessPartnerDto>>(businessPartner), "List is empty");

            var totalRecords = await _unitOfWork.BusinessPartner.TotalRecord(new BusinessPartnerSpecs(businessPartnerTypes, filter, true));

            return new PaginationResponse<List<BusinessPartnerDto>>(_mapper.Map<List<BusinessPartnerDto>>(businessPartner), filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<BusinessPartnerDto>> GetByIdAsync(int id)
        {
            var specification = new BusinessPartnerSpecs();
            var businessPartner = await _unitOfWork.BusinessPartner.GetById(id, specification);
            if (businessPartner == null)
                return new Response<BusinessPartnerDto>("Not found");

            return new Response<BusinessPartnerDto>(_mapper.Map<BusinessPartnerDto>(businessPartner), "Returning value");
        }

        public async Task<Response<BusinessPartnerDto>> UpdateAsync(CreateBusinessPartnerDto entity)
        {
            var businessPartner = await _unitOfWork.BusinessPartner.GetById((int)entity.Id);

            if (businessPartner == null)
                return new Response<BusinessPartnerDto>("Not found");

            var Receivablelevel4 = await _unitOfWork.Level4.GetById(entity.AccountReceivableId);

            //SBBU-Code
            //var AccountReceivable = ReceivableAndPayable.ValidateReceivable((Guid)Receivablelevel4.Level3_id);

            ////Validation for Receivable
            //if (AccountReceivable == false)
            //{
            //    return new Response<BusinessPartnerDto>("Account Receivable is Invalid");
            //}

            var Payablelevel4 = await _unitOfWork.Level4.GetById(entity.AccountPayableId);
            //SBBU-Code
            //var AccountPayable = ReceivableAndPayable.ValidatePayable((Guid)Payablelevel4.Level3_id);

            ////Validation for Payable
            //if (AccountPayable == false)
            //{
            //    return new Response<BusinessPartnerDto>("Account Payable is Invalid");
            //}
            //For updating data
            _mapper.Map<CreateBusinessPartnerDto, BusinessPartner>(entity, businessPartner);
            await _unitOfWork.SaveAsync();
            return new Response<BusinessPartnerDto>(_mapper.Map<BusinessPartnerDto>(businessPartner), "Updated successfully");
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<List<BusinessPartnerDto>>> GetBusinessPartnerDropDown()
        {
            var specification = new BusinessPartnerSpecs(true);

            var businessPartners = await _unitOfWork.BusinessPartner.GetAll(specification);
            if (!businessPartners.Any())
                return new Response<List<BusinessPartnerDto>>("List is empty");

            return new Response<List<BusinessPartnerDto>>(_mapper.Map<List<BusinessPartnerDto>>(businessPartners), "Returning List");
        }

        public Response<List<EmployeeBusinessPartnerDto>> GetAllBusinessPartnerDropDown()
        {
            // fetching business partner and employee with Employee Code
            var businessPartners = _unitOfWork.BusinessPartner.Find(new BusinessPartnerSpecs(1))
                .OrderByDescending(x => x.Id)
                .Select(e => new BusinessPartnerDto
                {
                Id = e.Id,
                Name = e.EmployeesList.Count() > 0 ? $"{e.EmployeesList.Select(i => i.EmployeeCode).FirstOrDefault()} - {e.Name}" : e.Name,
                BusinessPartnerType = e.BusinessPartnerType
                })
                .ToList();

            if (!businessPartners.Any())
                return new Response<List<EmployeeBusinessPartnerDto>>("List is empty");

            var bPDto = businessPartners.GroupBy(e => e.BusinessPartnerType)
                .Select(i => new EmployeeBusinessPartnerDto()
                {
                    Type = i.Key.ToString(),
                    BusinessPartner = i.ToList()
                })
                .ToList();

            return new Response<List<EmployeeBusinessPartnerDto>>(_mapper.Map<List<EmployeeBusinessPartnerDto>>(bPDto), "Returning List");
        }
    }
}
