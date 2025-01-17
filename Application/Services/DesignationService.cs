﻿using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using AutoMapper;
using Infrastructure.Specifications;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class DesignationService : IDesignationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DesignationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<DesignationDto>> CreateAsync(DesignationDto[] entity)
        {
            List<Designation> designationtList = new List<Designation>();
            foreach (var item in entity)
            {
                if(item.Id == null)
                {
                    designationtList.Add(_mapper.Map<Designation>(item));
                }
                else
                {
                     var getDesignation = await _unitOfWork.Designation.GetById((int)item.Id);

                if (getDesignation != null)
                {
                    _mapper.Map<DesignationDto, Designation>(item, getDesignation);
                    await _unitOfWork.SaveAsync();
                }
                else
                {
                    designationtList.Add(_mapper.Map<Designation>(item));
                }
                }
               
            }

            if (designationtList.Any())
            {
                await _unitOfWork.Designation.AddRange(designationtList);
                await _unitOfWork.SaveAsync();
            }

            return new Response<DesignationDto>(null, "Records populated successfully");
        }

        public async Task<PaginationResponse<List<DesignationDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var designations = await _unitOfWork.Designation.GetAll(new DesignationSpecs(filter, false));

            if (designations.Count() == 0)
                return new PaginationResponse<List<DesignationDto>>(_mapper.Map<List<DesignationDto>>(designations), "List is empty");

            var totalRecords = await _unitOfWork.Designation.TotalRecord(new DesignationSpecs(filter, true));

            return new PaginationResponse<List<DesignationDto>>(_mapper.Map<List<DesignationDto>>(designations), filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<DesignationDto>> GetByIdAsync(int id)
        {
            var designation = await _unitOfWork.Designation.GetById(id);
            if (designation == null)
                return new Response<DesignationDto>("Not found");

            return new Response<DesignationDto>(_mapper.Map<DesignationDto>(designation), "Returning value");
        }

        public async Task<Response<List<DesignationDto>>> GetDesignationDropDown()
        {
            var designations = await _unitOfWork.Designation.GetAll();
            if (!designations.Any())
                return new Response<List<DesignationDto>>("List is empty");

            return new Response<List<DesignationDto>>(_mapper.Map<List<DesignationDto>>(designations), "Returning List");
        }

        public Task<Response<DesignationDto>> UpdateAsync(DesignationDto[] entity)
        {
            throw new NotImplementedException();
        }
        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

    }
}
