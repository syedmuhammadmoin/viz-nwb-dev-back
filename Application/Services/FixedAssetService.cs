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
    public class FixedAssetService : IFixedAssetService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public FixedAssetService(IUnitOfWork unitOfWork , IMapper mapper) 
        {
         _mapper= mapper;
         _unitOfWork= unitOfWork;    
        }
        public async Task<Response<FixedAssetDto>> CreateAsync(CreateFixedAssetDto entity)
        {
            if (entity.DepreciationApplicability)
            {
                if (entity.DepreciationId == null && entity.DepreciationId == 0 || entity.AssetAccountId == null
                    || entity.DepreciationExpenseId == null || entity.AccumulatedDepreciationId == null ||
                    entity.UseFullLife == null)
                {
                    return new Response<FixedAssetDto>("Depreciation Model Fields are Required");
                }

                if (entity.ModelType == DepreciationMethod.Declining && entity.DecLiningRate == null)
                {
                    return new Response<FixedAssetDto>("Declining Rate is Required");
                }
            }
            else
            {
                entity.DepreciationId = null;
                entity.AssetAccountId = null;
                entity.UseFullLife = null;
            }
            var fixedAsset = _mapper.Map<FixedAsset>(entity);
            _unitOfWork.CreateTransaction();
            //Saving in table
            var result = await _unitOfWork.FixedAsset.Add(fixedAsset);
            await _unitOfWork.SaveAsync();

            //Commiting the transaction 
            _unitOfWork.Commit();

            //returning response
            return new Response<FixedAssetDto>(_mapper.Map<FixedAssetDto>(result), "Created successfully");
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PaginationResponse<List<FixedAssetDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var fixedAsset = await _unitOfWork.FixedAsset.GetAll(new FixedAssetSpecs(filter, false));

            if (fixedAsset.Count() == 0)
                return new PaginationResponse<List<FixedAssetDto>>(_mapper.Map<List<FixedAssetDto>>(fixedAsset), "List is empty");

            var totalRecords = await _unitOfWork.FixedAsset.TotalRecord(new FixedAssetSpecs(filter, true));

            return new PaginationResponse<List<FixedAssetDto>>(_mapper.Map<List<FixedAssetDto>>(fixedAsset),
                filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<FixedAssetDto>> GetByIdAsync(int id)
        {
            var fixedAsset = await _unitOfWork.FixedAsset.GetById(id, new FixedAssetSpecs());
            if (fixedAsset == null)
                return new Response<FixedAssetDto>("Not found");
            var fixedAssetDto = _mapper.Map<FixedAssetDto>(fixedAsset);
            return new Response<FixedAssetDto>(fixedAssetDto, "Returning value");
        }

        public async Task<Response<FixedAssetDto>> UpdateAsync(CreateFixedAssetDto entity)
        {
            var fixedAsset = await _unitOfWork.FixedAsset.GetById((int)entity.Id, new FixedAssetSpecs());
            if (fixedAsset == null)
                return new Response<FixedAssetDto>("Not found");

            if (entity.DepreciationApplicability)
            {
                if (entity.DepreciationId == null || entity.AssetAccountId == null
                    || entity.DepreciationExpenseId == null || entity.AccumulatedDepreciationId == null ||
                    entity.UseFullLife == null)
                {
                    return new Response<FixedAssetDto>("Depreciation Model Fields are Required");

                }
                if (entity.ModelType == DepreciationMethod.Declining && entity.DecLiningRate == null)
                {
                    return new Response<FixedAssetDto>("Declining Rate is Required");
                }
            }
            //For updating data
            _mapper.Map<CreateFixedAssetDto, FixedAsset>(entity, fixedAsset);

            _unitOfWork.CreateTransaction();

            await _unitOfWork.SaveAsync();

            //Commiting the transaction
            _unitOfWork.Commit();

            //returning response
            return new Response<FixedAssetDto>(_mapper.Map<FixedAssetDto>(fixedAsset), "Updated successfully");
        }
    }
}
