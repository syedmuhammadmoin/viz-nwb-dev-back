using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Specifications;

namespace Application.Services
{
    public class CityService : ICityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CityService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginationResponse<List<CityDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var result = await _unitOfWork.City.GetAll(new CitySpecs(filter, false));
            if (result.Count() == 0)
                return new PaginationResponse<List<CityDto>>(_mapper.Map<List<CityDto>>(result), "List is empty");

            var totalRecords = await _unitOfWork.City.TotalRecord(new CitySpecs(filter, true));
            return new PaginationResponse<List<CityDto>>(_mapper.Map<List<CityDto>>(result), filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<CityDto>> GetByIdAsync(int id)
        {
            var result = await _unitOfWork.City.GetById(id, new CitySpecs());
            if (result == null)
                return new Response<CityDto>("Not found");

            return new Response<CityDto>(_mapper.Map<CityDto>(result), "Returning value");
        }

        public async Task<Response<List<CityDto>>> GetDropDown()
        {
            var result = await _unitOfWork.City.GetAll();
            if (!result.Any())
                return new Response<List<CityDto>>(null, "List is empty");

            return new Response<List<CityDto>>(_mapper.Map<List<CityDto>>(result), "Returning List");
        }

        public async Task<Response<List<CityDto>>> GetByState(int stateId)
        {
            var result = await _unitOfWork.City.GetAll(new CitySpecs(stateId));
            if (!result.Any())
                return new Response<List<CityDto>>(null, "List is empty");

            return new Response<List<CityDto>>(_mapper.Map<List<CityDto>>(result), "Returning List");
        }

        public async Task<Response<CityDto>> CreateAsync(CreateCityDto entity)
        {
            var result = await _unitOfWork.City.Add(_mapper.Map<City>(entity));
            await _unitOfWork.SaveAsync();
            return new Response<CityDto>(_mapper.Map<CityDto>(result), "Created successfully");
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<CityDto>> UpdateAsync(CreateCityDto entity)
        {
            var result = await _unitOfWork.City.GetById((int)entity.Id);
            if (result == null)
                return new Response<CityDto>("Not found");

            //For updating data
            _mapper.Map(entity, result);
            await _unitOfWork.SaveAsync();
            return new Response<CityDto>(_mapper.Map<CityDto>(result), "Updated successfully");
        }

    }
}
