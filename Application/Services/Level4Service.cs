﻿using Application.Contracts.DTOs;
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
    public class Level4Service : ILevel4Service
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public Level4Service(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Response<Level4Dto>> CreateAsync(CreateLevel4Dto entity)
        {

            var level4 = _mapper.Map<Level4>(entity);
            var result = await _unitOfWork.Level4.Add(level4);
            await _unitOfWork.SaveAsync();
            return new Response<Level4Dto>(_mapper.Map<Level4Dto>(result), "Created successfully");
        }

        public async Task<PaginationResponse<List<Level4Dto>>> GetAllAsync(PaginationFilter filter)
        {

            var specification = new Level4Specs(filter);
            var level4 = await _unitOfWork.Level4.GetAll(specification);

            if (level4.Count() == 0)
                return new PaginationResponse<List<Level4Dto>>("List is empty");

            var totalRecords = await _unitOfWork.Level4.TotalRecord();

            return new PaginationResponse<List<Level4Dto>>(_mapper.Map<List<Level4Dto>>(level4), filter.PageStart, filter.PageEnd, totalRecords, "Returing list");

        }

        public async Task<Response<Level4Dto>> GetByIdAsync(Guid id)
        {
            var specification = new Level4Specs();
            var level4 = await _unitOfWork.Level4.GetById(id, specification);
            if (level4 == null)
                return new Response<Level4Dto>("Not found");

            return new Response<Level4Dto>(_mapper.Map<Level4Dto>(level4), "Returning value");
        }

        public async Task<Response<Level4Dto>> UpdateAsync(CreateLevel4Dto entity)
        {
            var level4 = await _unitOfWork.Level4.GetById((Guid)entity.Id);

            if (level4 == null)
                return new Response<Level4Dto>("Not found");

            //For updating data
            _mapper.Map<CreateLevel4Dto, Level4>(entity, level4);
            await _unitOfWork.SaveAsync();
            return new Response<Level4Dto>(_mapper.Map<Level4Dto>(level4), "Updated successfully");
        }
        public Task<Response<Guid>> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}