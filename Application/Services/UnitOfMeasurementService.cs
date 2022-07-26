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
    public class UnitOfMeasurementService : IUnitOfMeasurementService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UnitOfMeasurementService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<UnitOfMeasurementDto>> CreateAsync(CreateUnitOfMeasurementDto entity)
        {
            var unitOfMeasurement = _mapper.Map<UnitOfMeasurement>(entity);
            var result = await _unitOfWork.UnitOfMeasurement.Add(unitOfMeasurement);
            await _unitOfWork.SaveAsync();

            return new Response<UnitOfMeasurementDto>(_mapper.Map<UnitOfMeasurementDto>(result), "Created successfully");
        }

        public async Task<PaginationResponse<List<UnitOfMeasurementDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var unitOfMeasurements = await _unitOfWork.UnitOfMeasurement.GetAll(new UnitOfMeasurementSpecs(filter, false));

            if (unitOfMeasurements.Count() == 0)
                return new PaginationResponse<List<UnitOfMeasurementDto>>(_mapper.Map<List<UnitOfMeasurementDto>>(unitOfMeasurements), "List is empty");

            var totalRecords = await _unitOfWork.UnitOfMeasurement.TotalRecord(new UnitOfMeasurementSpecs(filter, true));

            return new PaginationResponse<List<UnitOfMeasurementDto>>(_mapper.Map<List<UnitOfMeasurementDto>>(unitOfMeasurements), filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<UnitOfMeasurementDto>> GetByIdAsync(int id)
        {
            var specification = new UnitOfMeasurementSpecs();
            var unitOfMeasurement = await _unitOfWork.UnitOfMeasurement.GetById(id, specification);
            if (unitOfMeasurement == null)
                return new Response<UnitOfMeasurementDto>("Not found");

            return new Response<UnitOfMeasurementDto>(_mapper.Map<UnitOfMeasurementDto>(unitOfMeasurement), "Returning value");
        }

        public async Task<Response<List<UnitOfMeasurementDto>>> GetUnitOfMeasurementDropDown()
        {
            var unitOfMeasurements = await _unitOfWork.UnitOfMeasurement.GetAll();
            if (!unitOfMeasurements.Any())
                return new Response<List<UnitOfMeasurementDto>>("List is empty");

            return new Response<List<UnitOfMeasurementDto>>(_mapper.Map<List<UnitOfMeasurementDto>>(unitOfMeasurements), "Returning List");
        }

        public async Task<Response<UnitOfMeasurementDto>> UpdateAsync(CreateUnitOfMeasurementDto entity)
        {
            var tax = await _unitOfWork.UnitOfMeasurement.GetById((int)entity.Id);

            if (tax == null)
                return new Response<UnitOfMeasurementDto>("Not found");

            //For updating data
            _mapper.Map<CreateUnitOfMeasurementDto, UnitOfMeasurement>(entity, tax);
            await _unitOfWork.SaveAsync();
            return new Response<UnitOfMeasurementDto>(_mapper.Map<UnitOfMeasurementDto>(tax), "Updated successfully");
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
