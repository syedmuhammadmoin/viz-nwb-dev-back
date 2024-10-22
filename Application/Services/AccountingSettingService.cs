using Application.Contracts.DTOs.AccountSetting;
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
    public class AccountingSettingService : IAccountingSettingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AccountingSettingService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Response<AccountingSettingDto>> CreateAsync(CreateAccountingSettingDto entity)
        {
            _unitOfWork.TaxSetting.DeleteAll();

            var result = await _unitOfWork.AccountingSetting.Add(_mapper.Map<AccountingSettingEntity>(entity));
            await _unitOfWork.SaveAsync();
            return new Response<AccountingSettingDto>(_mapper.Map<AccountingSettingDto>(result), "Created Successfully.");
        }

        public async Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    

        public async Task<PaginationResponse<List<AccountingSettingDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var FsRecords = await _unitOfWork.AccountingSetting.GetAll();

            if (FsRecords.Count() == 0)
                return new PaginationResponse<List<AccountingSettingDto>>(_mapper.Map<List<AccountingSettingDto>>(FsRecords), "List is empty");

            var totalRecords = await _unitOfWork.AccountingSetting.TotalRecord();

            return new PaginationResponse<List<AccountingSettingDto>>(_mapper.Map<List<AccountingSettingDto>>(FsRecords), filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<AccountingSettingDto>> GetByIdAsync(int id)
        {
            var specs = new AccountingSettingSpecs(true);
            var tax = await _unitOfWork.AccountingSetting.GetById(id);
            if (tax == null)
                return new Response<AccountingSettingDto>("Not found");

            return new Response<AccountingSettingDto>(_mapper.Map<AccountingSettingDto>(tax), "Returning value");
        }

        public async Task<Response<AccountingSettingDto>> UpdateAsync(CreateAccountingSettingDto entity)
        {
            throw new NotImplementedException();
        }
    }
}
