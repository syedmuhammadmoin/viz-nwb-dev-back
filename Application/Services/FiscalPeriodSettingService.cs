using Application.Contracts.DTOs.FiscalPeriod;
using Application.Contracts.DTOs.FiscalPeriodSetting;
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
    public class FiscalPeriodSettingService : IFiscalPeriodSettingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FiscalPeriodSettingService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Response<FiscalPeriodSettingDto>> CreateAsync(CreateFiscalPeriodSettingDto entity)
        {
            var result = await _unitOfWork.FiscalPeriodSetting.Add(_mapper.Map<FiscalPeriodSetting>(entity));
            await _unitOfWork.SaveAsync();
            return new Response<FiscalPeriodSettingDto>(_mapper.Map<FiscalPeriodSettingDto>(result), "Created Successfully.");
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PaginationResponse<List<FiscalPeriodSettingDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var FsRecords = await _unitOfWork.FiscalPeriodSetting.GetAll();

            if (FsRecords.Count() == 0)
                return new PaginationResponse<List<FiscalPeriodSettingDto>>(_mapper.Map<List<FiscalPeriodSettingDto>>(FsRecords), "List is empty");

            var totalRecords = await _unitOfWork.FiscalPeriod.TotalRecord(new FiscalPeriodSpecs(filter, true));

            return new PaginationResponse<List<FiscalPeriodSettingDto>>(_mapper.Map<List<FiscalPeriodSettingDto>>(FsRecords), filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<FiscalPeriodSettingDto>> GetByIdAsync(int id)
        {           
            var tax = await _unitOfWork.FiscalPeriod.GetById(id);
            if (tax == null)
                return new Response<FiscalPeriodSettingDto>("Not found");

            return new Response<FiscalPeriodSettingDto>(_mapper.Map<FiscalPeriodSettingDto>(tax), "Returning value");
        }

        public async Task<Response<FiscalPeriodSettingDto>> UpdateAsync(CreateFiscalPeriodSettingDto entity)
        {
            var result = await _unitOfWork.FiscalPeriodSetting.GetById(entity.Id);
            if (result == null)
                return new Response<FiscalPeriodSettingDto>("No Fiscal Year Found");
            _mapper.Map(entity, result);
            await _unitOfWork.SaveAsync();
            return new Response<FiscalPeriodSettingDto>(_mapper.Map<FiscalPeriodSettingDto>(entity), "Updated Successfully");
        }
    }
}
