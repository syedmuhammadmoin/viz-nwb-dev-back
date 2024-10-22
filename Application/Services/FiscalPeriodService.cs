using Application.Contracts.DTOs;
using Application.Contracts.DTOs.FiscalPeriod;
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
    public class FiscalPeriodService : IFiscalPeriodService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FiscalPeriodService(IUnitOfWork unitOfWork , IMapper mapper)
        {
           _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Response<FiscalPeriodDto>> CreateAsync(CreateFiscalPeriodDto entity)
        {
            var result = await _unitOfWork.FiscalPeriod.Add(_mapper.Map<FiscalPeriod>(entity));
            await _unitOfWork.SaveAsync(); 
            return new Response<FiscalPeriodDto>(_mapper.Map<FiscalPeriodDto>(result),"Created Successfully.");
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PaginationResponse<List<FiscalPeriodDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var FsRecords = await _unitOfWork.FiscalPeriod.GetAll(new FiscalPeriodSpecs(filter, false));

            if (FsRecords.Count() == 0)
                return new PaginationResponse<List<FiscalPeriodDto>>(_mapper.Map<List<FiscalPeriodDto>>(FsRecords), "List is empty");

            var totalRecords = await _unitOfWork.FiscalPeriod.TotalRecord(new FiscalPeriodSpecs(filter, true));

            return new PaginationResponse<List<FiscalPeriodDto>>(_mapper.Map<List<FiscalPeriodDto>>(FsRecords), filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<FiscalPeriodDto>> GetByIdAsync(int id)
        {
            var specification = new FiscalPeriodSpecs();
            var tax = await _unitOfWork.FiscalPeriod.GetById(id, specification);
            if (tax == null)
                return new Response<FiscalPeriodDto>("Not found");

            return new Response<FiscalPeriodDto>(_mapper.Map<FiscalPeriodDto>(tax), "Returning value");
        }

        public async Task<Response<FiscalPeriodDto>> UpdateAsync(CreateFiscalPeriodDto entity)
        {
            var result = await _unitOfWork.FiscalPeriod.GetById(entity.Id);
            if (result == null)
                return new Response<FiscalPeriodDto>("No Fiscal Year Found");
            _mapper.Map(entity, result);
            await _unitOfWork.SaveAsync();
            return new Response<FiscalPeriodDto>(_mapper.Map<FiscalPeriodDto>(entity), "Updated Successfully");
        }
        public async Task<Response<bool>> DeleteBulkRecords(List<int> ids)
        {
            if (ids.Count() < 0 || ids == null)
            {
                return new Response<bool>("List Cannot be Empty.");
            }
            else
            {
                foreach (var id in ids)
                {
                    var record = await _unitOfWork.FiscalPeriod.GetById(id);
                    if (record == null)
                        return new Response<bool>("Tax Not Found.");
                    record.IsDelete = true;
                    await _unitOfWork.SaveAsync();
                }
                return new Response<bool>(true, "Deleted Successfully.");
            }
        }
    }
}
