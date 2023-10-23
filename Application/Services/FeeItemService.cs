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
    public class FeeItemService : IFeeItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FeeItemService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginationResponse<List<FeeItemDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var result = await _unitOfWork.FeeItem.GetAll(new FeeItemSpecs(filter, false));
            if (result.Count() == 0)
                return new PaginationResponse<List<FeeItemDto>>(_mapper.Map<List<FeeItemDto>>(result), "List is empty");

            var totalRecords = await _unitOfWork.FeeItem.TotalRecord(new FeeItemSpecs(filter, true));
            return new PaginationResponse<List<FeeItemDto>>(_mapper.Map<List<FeeItemDto>>(result), filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<FeeItemDto>> GetByIdAsync(int id)
        {
            var result = await _unitOfWork.FeeItem.GetById(id, new FeeItemSpecs());
            if (result == null)
                return new Response<FeeItemDto>("Not found");

            return new Response<FeeItemDto>(_mapper.Map<FeeItemDto>(result), "Returning value");
        }

        public async Task<Response<List<FeeItemDto>>> GetDropDown()
        {
            var result = await _unitOfWork.FeeItem.GetAll();
            if (!result.Any())
                return new Response<List<FeeItemDto>>(null, "List is empty");

            return new Response<List<FeeItemDto>>(_mapper.Map<List<FeeItemDto>>(result), "Returning List");
        }

        public async Task<Response<FeeItemDto>> CreateAsync(CreateFeeItemDto entity)
        {
            var result = await _unitOfWork.FeeItem.Add(_mapper.Map<FeeItem>(entity));
            await _unitOfWork.SaveAsync();
            return new Response<FeeItemDto>(_mapper.Map<FeeItemDto>(result), "Created successfully");
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<FeeItemDto>> UpdateAsync(CreateFeeItemDto entity)
        {
            var result = await _unitOfWork.FeeItem.GetById((int)entity.Id);
            if (result == null)
                return new Response<FeeItemDto>("Not found");

            //For updating data
            _mapper.Map(entity, result);
            await _unitOfWork.SaveAsync();
            return new Response<FeeItemDto>(_mapper.Map<FeeItemDto>(result), "Updated successfully");
        }
    }
}
