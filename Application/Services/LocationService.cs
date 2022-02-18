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
    public class LocationService  : ILocationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LocationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<LocationDto>> CreateAsync(CreateLocationDto entity)
        {
            var location = _mapper.Map<Location>(entity);
            var result = await _unitOfWork.Location.Add(location);
            await _unitOfWork.SaveAsync();

            return new Response<LocationDto>(_mapper.Map<LocationDto>(result), "Created successfully");
        }

        public async Task<PaginationResponse<List<LocationDto>>> GetAllAsync(PaginationFilter filter)
        {
            var specification = new LocationSpecs(filter);
            var location = await _unitOfWork.Location.GetAll(specification);

            if (location.Count() == 0)
                return new PaginationResponse<List<LocationDto>>("List is empty");

            var totalRecords = await _unitOfWork.Location.TotalRecord();

            return new PaginationResponse<List<LocationDto>>(_mapper.Map<List<LocationDto>>(location), filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
           
        }

        public async Task<Response<LocationDto>> GetByIdAsync(int id)
        {
            var location = await _unitOfWork.Location.GetById(id);
            if (location == null)
                return new Response<LocationDto>("Not found");

            return new Response<LocationDto>(_mapper.Map<LocationDto>(location), "Returning value");
        }

        public async Task<Response<LocationDto>> UpdateAsync(CreateLocationDto entity)
        {
            var location = await _unitOfWork.Location.GetById((int)entity.Id);

            if (location == null)
                return new Response<LocationDto>("Not found");

            //For updating data
            _mapper.Map<CreateLocationDto, Location>(entity, location);
            await _unitOfWork.SaveAsync();
            return new Response<LocationDto>(_mapper.Map<LocationDto>(location), "Updated successfully");
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
