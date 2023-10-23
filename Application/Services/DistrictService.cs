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
    public class DistrictService : IDistrictService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DistrictService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginationResponse<List<DistrictDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var result = await _unitOfWork.District.GetAll(new DistrictSpecs(filter, false));
            if (result.Count() == 0)
                return new PaginationResponse<List<DistrictDto>>(_mapper.Map<List<DistrictDto>>(result), "List is empty");

            var totalRecords = await _unitOfWork.District.TotalRecord(new DistrictSpecs(filter, true));
            return new PaginationResponse<List<DistrictDto>>(_mapper.Map<List<DistrictDto>>(result), filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<DistrictDto>> GetByIdAsync(int id)
        {
            var result = await _unitOfWork.District.GetById(id, new DistrictSpecs());
            if (result == null)
                return new Response<DistrictDto>("Not found");

            return new Response<DistrictDto>(_mapper.Map<DistrictDto>(result), "Returning value");
        }

        public async Task<Response<List<DistrictDto>>> GetDropDown()
        {
            var result = await _unitOfWork.District.GetAll();
            if (!result.Any())
                return new Response<List<DistrictDto>>(null, "List is empty");

            return new Response<List<DistrictDto>>(_mapper.Map<List<DistrictDto>>(result), "Returning List");
        }

        public async Task<Response<List<DistrictDto>>> GetByCity(int cityId)
        {
            var result = await _unitOfWork.District.GetAll(new DistrictSpecs(cityId));
            if (!result.Any())
                return new Response<List<DistrictDto>>(null, "List is empty");

            return new Response<List<DistrictDto>>(_mapper.Map<List<DistrictDto>>(result), "Returning List");
        }

        public async Task<Response<DistrictDto>> CreateAsync(CreateDistrictDto entity)
        {
            var result = await _unitOfWork.District.Add(_mapper.Map<District>(entity));
            await _unitOfWork.SaveAsync();
            return new Response<DistrictDto>(_mapper.Map<DistrictDto>(result), "Created successfully");
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<DistrictDto>> UpdateAsync(CreateDistrictDto entity)
        {
            var result = await _unitOfWork.District.GetById((int)entity.Id);
            if (result == null)
                return new Response<DistrictDto>("Not found");

            //For updating data
            _mapper.Map(entity, result);
            await _unitOfWork.SaveAsync();
            return new Response<DistrictDto>(_mapper.Map<DistrictDto>(result), "Updated successfully");
        }

    }
}
