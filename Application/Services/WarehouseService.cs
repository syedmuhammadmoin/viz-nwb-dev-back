﻿using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Response;
using Application.Contracts.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Specifications;

namespace Application.Services
{
    public class WarehouseService : IWarehouseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public WarehouseService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<WarehouseDto>> CreateAsync(CreateWarehouseDto entity)
        {
            var warehouse = _mapper.Map<Warehouse>(entity);
            var result = await _unitOfWork.Warehouse.Add(warehouse);
            await _unitOfWork.SaveAsync();

            return new Response<WarehouseDto>(_mapper.Map<WarehouseDto>(result), "Created successfully");
        }

        public async Task<PaginationResponse<List<WarehouseDto>>> GetAllAsync(PaginationFilter filter)
        {
            var specification = new WarehouseSpecs(filter);
            var warehouse = await _unitOfWork.Warehouse.GetAll(specification);

            if (warehouse.Count() == 0)
                return new PaginationResponse<List<WarehouseDto>>("List is empty");

            var totalRecords = await _unitOfWork.Warehouse.TotalRecord();

            return new PaginationResponse<List<WarehouseDto>>(_mapper.Map<List<WarehouseDto>>(warehouse), filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<WarehouseDto>> GetByIdAsync(int id)
        {
            var specification = new WarehouseSpecs();
            var warehouse = await _unitOfWork.Warehouse.GetById(id, specification);
            if (warehouse == null)
                return new Response<WarehouseDto>("Not found");

            return new Response<WarehouseDto>(_mapper.Map<WarehouseDto>(warehouse), "Returning value");
        }

        public async Task<Response<WarehouseDto>> UpdateAsync(CreateWarehouseDto entity)
        {
            var warehouse = await _unitOfWork.Warehouse.GetById((int)entity.Id);

            if (warehouse == null)
                return new Response<WarehouseDto>("Not found");

            //For updating data
            _mapper.Map<CreateWarehouseDto, Warehouse>(entity, warehouse);
            await _unitOfWork.SaveAsync();
            return new Response<WarehouseDto>(_mapper.Map<WarehouseDto>(warehouse), "Updated successfully");
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}