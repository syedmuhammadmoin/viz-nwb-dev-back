using Application.Contracts.DTOs.CurrencySetting;
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
    public class CurrencySettingService : ICurrencySettingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CurrencySettingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Response<DefaultCurrencyDto>> CreateAsync(CreateDefaultCurrencySettingDto entity)
        {
            var result = await _unitOfWork.CurrencySetting.Add(_mapper.Map<DefaultCurrencySetting>(entity));
            await _unitOfWork.SaveAsync();
            return new Response<DefaultCurrencyDto>(_mapper.Map<DefaultCurrencyDto>(result), "Created Successfully.");
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PaginationResponse<List<DefaultCurrencyDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var CRRecords = await _unitOfWork.CurrencySetting.GetAll();

            if (CRRecords.Count() == 0)
                return new PaginationResponse<List<DefaultCurrencyDto>>(_mapper.Map<List<DefaultCurrencyDto>>(CRRecords), "List is empty");

            var totalRecords = await _unitOfWork.CurrencySetting.TotalRecord();

            return new PaginationResponse<List<DefaultCurrencyDto>>(_mapper.Map<List<DefaultCurrencyDto>>(CRRecords), filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<DefaultCurrencyDto>> GetByIdAsync(int id)
        {
            var tax = await _unitOfWork.CurrencySetting.GetById(id);
            if (tax == null)
                return new Response<DefaultCurrencyDto>("Not found");

            return new Response<DefaultCurrencyDto>(_mapper.Map<DefaultCurrencyDto>(tax), "Returning value");
        }

        public async Task<Response<DefaultCurrencyDto>> UpdateAsync(CreateDefaultCurrencySettingDto entity)
        {
            var result = await _unitOfWork.CurrencySetting.GetById(entity.Id);
            if (result == null)
                return new Response<DefaultCurrencyDto>("No Setting Found");
            _mapper.Map(entity, result);
            await _unitOfWork.SaveAsync();
            return new Response<DefaultCurrencyDto>(_mapper.Map<DefaultCurrencyDto>(entity), "Updated Successfully");
        }
    }
}
