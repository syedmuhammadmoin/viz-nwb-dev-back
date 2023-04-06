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
    public class BatchService : IBatchService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BatchService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginationResponse<List<BatchDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var result = await _unitOfWork.Batch.GetAll(new BatchSpecs(filter, false));
            if (result.Count() == 0)
                return new PaginationResponse<List<BatchDto>>(_mapper.Map<List<BatchDto>>(result), "List is empty");

            var totalRecords = await _unitOfWork.Batch.TotalRecord(new BatchSpecs(filter, true));
            return new PaginationResponse<List<BatchDto>>(_mapper.Map<List<BatchDto>>(result), filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<BatchDto>> GetByIdAsync(int id)
        {
            var result = await _unitOfWork.Batch.GetById(id, new BatchSpecs(false));
            if (result == null)
                return new Response<BatchDto>("Not found");

            return new Response<BatchDto>(_mapper.Map<BatchDto>(result), "Returning value");
        }

        public async Task<Response<List<BatchDto>>> GetDropDown()
        {
            var result = await _unitOfWork.Batch.GetAll();
            if (!result.Any())
                return new Response<List<BatchDto>>(null, "List is empty");

            return new Response<List<BatchDto>>(_mapper.Map<List<BatchDto>>(result), "Returning List");
        }

        public async Task<Response<BatchDto>> CreateAsync(CreateBatchDto entity)
        {
            var result = await _unitOfWork.Batch.Add(_mapper.Map<BatchMaster>(entity));
            await _unitOfWork.SaveAsync();
            return new Response<BatchDto>(_mapper.Map<BatchDto>(result), "Created successfully");
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<BatchDto>> UpdateAsync(CreateBatchDto entity)
        {
            var result = await _unitOfWork.Batch.GetById((int)entity.Id, new BatchSpecs(true));
            if (result == null)
                return new Response<BatchDto>("Not found");

            //For updating data
            _mapper.Map(entity, result);
            await _unitOfWork.SaveAsync();
            return new Response<BatchDto>(_mapper.Map<BatchDto>(result), "Updated successfully");
        }

    }
}
