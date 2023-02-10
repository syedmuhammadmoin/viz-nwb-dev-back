using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using AutoMapper;
using Domain.Constants;
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
    public class DepreciationService : IDepreciationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepreciationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<DepreciationDto>> CreateAsync(CreateDepreciationDto entity)
        {
            var depreciation = _mapper.Map<Depreciation>(entity);
         
            _unitOfWork.CreateTransaction();
            //Saving in table
            var result = await _unitOfWork.Depreciation.Add(depreciation);
            await _unitOfWork.SaveAsync();

            //Commiting the transaction 
            _unitOfWork.Commit();

            //returning response
            return new Response<DepreciationDto>(_mapper.Map<DepreciationDto>(result), "Created successfully");
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PaginationResponse<List<DepreciationDto>>> GetAllAsync(TransactionFormFilter filter)
        {

            var depreciation = await _unitOfWork.Depreciation.GetAll(new DepreciationSpecs(filter, false));

            if (depreciation.Count() == 0)
                return new PaginationResponse<List<DepreciationDto>>(_mapper.Map<List<DepreciationDto>>(depreciation), "List is empty");

            var totalRecords = await _unitOfWork.Depreciation.TotalRecord(new DepreciationSpecs(filter, true));

            return new PaginationResponse<List<DepreciationDto>>(_mapper.Map<List<DepreciationDto>>(depreciation),
                filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<DepreciationDto>> GetByIdAsync(int id)
        {
            var depreciation = await _unitOfWork.Depreciation.GetById(id, new DepreciationSpecs());
            if (depreciation == null)
                return new Response<DepreciationDto>("Not found");
            var depreciationDto = _mapper.Map<DepreciationDto>(depreciation);
            return new Response<DepreciationDto>(depreciationDto, "Returning value"); 
        }

        public async Task<Response<List<DepreciationDto>>> GetDepreciationDown()
        {
            var depreciations = await _unitOfWork.Depreciation.GetAll();
            if (!depreciations.Any())
                return new Response<List<DepreciationDto>>("List is empty");

            return new Response<List<DepreciationDto>>(_mapper.Map<List<DepreciationDto>>(depreciations), "Returning List");
        }

        public async Task<Response<DepreciationDto>> UpdateAsync(CreateDepreciationDto entity)
        {
            var depreciation = await _unitOfWork.Depreciation.GetById((int)entity.Id, new DepreciationSpecs());
            if (depreciation == null)
                return new Response<DepreciationDto>("Not found");
            //For updating data
            _mapper.Map<CreateDepreciationDto, Depreciation>(entity, depreciation);

            _unitOfWork.CreateTransaction();

            await _unitOfWork.SaveAsync();

            //Commiting the transaction
            _unitOfWork.Commit();

            //returning response
            return new Response<DepreciationDto>(_mapper.Map<DepreciationDto>(depreciation), "Updated successfully");
        }
    }
}
