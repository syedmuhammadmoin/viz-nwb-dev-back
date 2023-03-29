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
    public class CountryService : ICountryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CountryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginationResponse<List<CountryDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var result = await _unitOfWork.Country.GetAll(new CountrySpecs(filter, false));
            if (result.Count() == 0)
                return new PaginationResponse<List<CountryDto>>(_mapper.Map<List<CountryDto>>(result), "List is empty");

            var totalRecords = await _unitOfWork.Country.TotalRecord(new CountrySpecs(filter, true));
            return new PaginationResponse<List<CountryDto>>(_mapper.Map<List<CountryDto>>(result), filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<CountryDto>> GetByIdAsync(int id)
        {
            var result = await _unitOfWork.Country.GetById(id);
            if (result == null)
                return new Response<CountryDto>("Not found");

            return new Response<CountryDto>(_mapper.Map<CountryDto>(result), "Returning value");
        }

        public async Task<Response<List<CountryDto>>> GetDropDown()
        {
            var result = await _unitOfWork.Country.GetAll();
            if (!result.Any())
                return new Response<List<CountryDto>>(null, "List is empty");

            return new Response<List<CountryDto>>(_mapper.Map<List<CountryDto>>(result), "Returning List");
        }

        public async Task<Response<CountryDto>> CreateAsync(CountryDto entity)
        {
            var result = await _unitOfWork.Country.Add(_mapper.Map<Country>(entity));
            await _unitOfWork.SaveAsync();
            return new Response<CountryDto>(_mapper.Map<CountryDto>(result), "Created successfully");
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<CountryDto>> UpdateAsync(CountryDto entity)
        {
            var result = await _unitOfWork.Country.GetById((int)entity.Id);
            if (result == null)
                return new Response<CountryDto>("Not found");

            //For updating data
            _mapper.Map(entity, result);
            await _unitOfWork.SaveAsync();
            return new Response<CountryDto>(_mapper.Map<CountryDto>(result), "Updated successfully");
        }
    }
}
