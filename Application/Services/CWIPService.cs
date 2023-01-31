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
    public class CWIPService : ICWIPService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CWIPService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<CWIPDto>> CreateAsync(CreateCWIPDto entity)
        {
            if (entity.DepreciationApplicability)
            {
                if (entity.DepreciationId == null && entity.DepreciationId == 0 || entity.DepreciationExpenseId == null ||
                    entity.UseFullLife == null)
                {
                    return new Response<CWIPDto>("Depreciation Model Fields are Required");
                }

                if (entity.ModelType == DepreciationMethod.Declining && entity.DecLiningRate == null)
                {
                    return new Response<CWIPDto>("Declining Rate is Required");
                }
            }
            else
            {
                entity.DepreciationId = null;
                entity.DepreciationExpenseId = null;
                entity.AccumulatedDepreciationId = null;
                entity.UseFullLife = null;
            }

            var cwip = _mapper.Map<CWIP>(entity);
            _unitOfWork.CreateTransaction();
            //Saving in table
            var result = await _unitOfWork.CWIP.Add(cwip);
            await _unitOfWork.SaveAsync();

            //Commiting the transaction 
            _unitOfWork.Commit();

            //returning response
            return new Response<CWIPDto>(_mapper.Map<CWIPDto>(result), "Created successfully");
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PaginationResponse<List<CWIPDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var cwip = await _unitOfWork.CWIP.GetAll(new CWIPSpecs(filter, false));

            if (cwip.Count() == 0)
                return new PaginationResponse<List<CWIPDto>>(_mapper.Map<List<CWIPDto>>(cwip), "List is empty");

            var totalRecords = await _unitOfWork.CWIP.TotalRecord(new CWIPSpecs(filter, true));

            return new PaginationResponse<List<CWIPDto>>(_mapper.Map<List<CWIPDto>>(cwip),
                filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<CWIPDto>> GetByIdAsync(int id)
        {
            var cwip = await _unitOfWork.CWIP.GetById(id, new CWIPSpecs());

            if (cwip == null)
                return new Response<CWIPDto>("Not found");

            var cwipDt0 = _mapper.Map<CWIPDto>(cwip);

            return new Response<CWIPDto>(cwipDt0, "Returning value");
        }

        public async Task<Response<CWIPDto>> UpdateAsync(CreateCWIPDto entity)
        {
            var cwip = await _unitOfWork.CWIP.GetById((int)entity.Id, new CWIPSpecs());
            if (cwip == null)
                return new Response<CWIPDto>("Not found");

            if (entity.DepreciationApplicability)
            {
                if (entity.DepreciationId == null || entity.DepreciationExpenseId == null || entity.AccumulatedDepreciationId == null ||
                    entity.UseFullLife == null)
                {
                    return new Response<CWIPDto>("Depreciation Model Fields are Required");

                }
                if (entity.ModelType == DepreciationMethod.Declining && entity.DecLiningRate == null)
                {
                    return new Response<CWIPDto>("Declining Rate is Required");
                }
            }
            //For updating data
            _mapper.Map<CreateCWIPDto, CWIP>(entity, cwip);

            _unitOfWork.CreateTransaction();

            await _unitOfWork.SaveAsync();

            //Commiting the transaction
            _unitOfWork.Commit();

            //returning response
            return new Response<CWIPDto>(_mapper.Map<CWIPDto>(cwip), "Updated successfully");
        }
    }
}
