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
    public class DepreciationModelService : IDepreciationModelService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepreciationModelService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<DepreciationModelDto>> CreateAsync(CreateDepreciationModelDto entity)
        {
            var depreciation = _mapper.Map<DepreciationModel>(entity);
         
            _unitOfWork.CreateTransaction();
            //Saving in table
            var result = await _unitOfWork.Depreciation.Add(depreciation);
            await _unitOfWork.SaveAsync();

            //Commiting the transaction 
            _unitOfWork.Commit();

            //returning response
            return new Response<DepreciationModelDto>(_mapper.Map<DepreciationModelDto>(result), "Created successfully");
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PaginationResponse<List<DepreciationModelDto>>> GetAllAsync(TransactionFormFilter filter)
        {

            var depreciation = await _unitOfWork.Depreciation.GetAll(new DepreciationModelSpecs(filter, false));

            if (depreciation.Count() == 0)
                return new PaginationResponse<List<DepreciationModelDto>>(_mapper.Map<List<DepreciationModelDto>>(depreciation), "List is empty");

            var totalRecords = await _unitOfWork.Depreciation.TotalRecord(new DepreciationModelSpecs(filter, true));

            return new PaginationResponse<List<DepreciationModelDto>>(_mapper.Map<List<DepreciationModelDto>>(depreciation),
                filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<DepreciationModelDto>> GetByIdAsync(int id)
        {
            var depreciation = await _unitOfWork.Depreciation.GetById(id, new DepreciationModelSpecs());
            if (depreciation == null)
                return new Response<DepreciationModelDto>("Not found");
            var depreciationDto = _mapper.Map<DepreciationModelDto>(depreciation);
            return new Response<DepreciationModelDto>(depreciationDto, "Returning value"); 
        }

        public async Task<Response<List<DepreciationModelDto>>> GetDepreciationDown()
        {
            var depreciations = await _unitOfWork.Depreciation.GetAll();
            if (!depreciations.Any())
                return new Response<List<DepreciationModelDto>>("List is empty");

            return new Response<List<DepreciationModelDto>>(_mapper.Map<List<DepreciationModelDto>>(depreciations), "Returning List");
        }

        public async Task<Response<DepreciationModelDto>> UpdateAsync(CreateDepreciationModelDto entity)
        {
            var depreciation = await _unitOfWork.Depreciation.GetById((int)entity.Id, new DepreciationModelSpecs());
            if (depreciation == null)
                return new Response<DepreciationModelDto>("Not found");
            //For updating data
            _mapper.Map<CreateDepreciationModelDto, DepreciationModel>(entity, depreciation);

            _unitOfWork.CreateTransaction();

            await _unitOfWork.SaveAsync();

            //Commiting the transaction
            _unitOfWork.Commit();

            //returning response
            return new Response<DepreciationModelDto>(_mapper.Map<DepreciationModelDto>(depreciation), "Updated successfully");
        }
    }
}
