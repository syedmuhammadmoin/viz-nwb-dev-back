﻿using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Helper;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Specifications;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CashAccountService : ICashAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public CashAccountService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Response<CashAccountDto>> CreateAsync(CreateCashAccountDto entity)
        {
            var checkingCode = _unitOfWork.Level4.Find(new Level4Specs(entity.AccountCode)).FirstOrDefault();
            if (checkingCode != null)
                return new Response<CashAccountDto>("Duplicate code");

            _unitOfWork.CreateTransaction();
           
                var ChAccount = new Level4(
                    entity.CashAccountName,
                    entity.AccountCode,
                    "12500000-5566-7788-99AA-BBCCDDEEFF00",
                    "10000000-5566-7788-99AA-BBCCDDEEFF00",
                    GetTenant.GetTenantId(_httpContextAccessor)
                    );

                await _unitOfWork.Level4.Add(ChAccount);

                var transaction = new Transactions(0, "1", DocType.CashAccount);
                await _unitOfWork.Transaction.Add(transaction);
                await _unitOfWork.SaveAsync();

                //Inserting into cashAccount table
                var cashAccount = _mapper.Map<CashAccount>(entity);

                cashAccount.SetChAccountId(ChAccount.Id);
                cashAccount.SetTransactionId(transaction.Id);

                await _unitOfWork.CashAccount.Add(cashAccount);
                await _unitOfWork.SaveAsync();

                cashAccount.CreateDocNo();
                transaction.UpdateDocNo(cashAccount.Id, cashAccount.DocNo);
                await _unitOfWork.SaveAsync();

                //Adding CashAccount to Ledger
                await AddToLedger(cashAccount);

                //Commiting the transaction 
                _unitOfWork.Commit();

                //returning response
                return new Response<CashAccountDto>(_mapper.Map<CashAccountDto>(cashAccount), "Created successfully");
        
        }

        public async Task<PaginationResponse<List<CashAccountDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var cashAccount = await _unitOfWork.CashAccount.GetAll(new CashAccountSpecs(filter, false));

            if (!cashAccount.Any())
                return new PaginationResponse<List<CashAccountDto>>(_mapper.Map<List<CashAccountDto>>(cashAccount), "List is empty");

            var totalRecords = await _unitOfWork.CashAccount.TotalRecord(new CashAccountSpecs(filter, true));

            return new PaginationResponse<List<CashAccountDto>>(_mapper.Map<List<CashAccountDto>>(cashAccount), filter.PageStart, filter.PageEnd, totalRecords, "Returing list");

        }

        public async Task<Response<CashAccountDto>> GetByIdAsync(int id)
        {
            var cashAccount = await _unitOfWork.CashAccount.GetById(id);
            if (cashAccount == null)
                return new Response<CashAccountDto>("Not found");

            return new Response<CashAccountDto>(_mapper.Map<CashAccountDto>(cashAccount), "Returning value");
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }


        public async Task<Response<CashAccountDto>> UpdateAsync(UpdateCashAccountDto entity)
        {
            var checkingCode = _unitOfWork.Level4.Find(new Level4Specs(entity.AccountCode)).FirstOrDefault();
            if (checkingCode != null)
                return new Response<CashAccountDto>("Duplicate code");

            _unitOfWork.CreateTransaction();
         
                var cashAccount = await _unitOfWork.CashAccount.GetById((int)entity.Id);

                if (cashAccount == null)
                    return new Response<CashAccountDto>("Not found");

                //For updating data
                _mapper.Map<UpdateCashAccountDto, CashAccount>(entity, cashAccount);

                // Getting account detail in COA
                var account = await _unitOfWork.Level4.GetById(cashAccount.ChAccountId);
                if (account == null)
                    return new Response<CashAccountDto>("Account not found in Chart Of Account");

                //Updating account name in chart of account
                account.SetAccountName(entity.CashAccountName, entity.AccountCode);


                await _unitOfWork.SaveAsync();
                _unitOfWork.Commit();
                return new Response<CashAccountDto>(_mapper.Map<CashAccountDto>(cashAccount), "Updated successfully");
       
        }

        private async Task AddToLedger(CashAccount cashAccount)
        {
            var addBalanceInCashAccount = new RecordLedger(
                cashAccount.TransactionId,
                cashAccount.ChAccountId,
                null,
                null,
                "Opening Balance",
                'D',
                cashAccount.OpeningBalance,
                cashAccount.CampusId,
                cashAccount.OpeningBalanceDate
                );

            await _unitOfWork.Ledger.Add(addBalanceInCashAccount);

            var addBalanceInOpeningBalanceEquity = new RecordLedger(
               cashAccount.TransactionId,
               "31210000-5566-7788-99AA-BBCCDDEEFF00" + $"-{GetTenant.GetTenantId(_httpContextAccessor)}",
               null,
               null,
               "Opening Balance",
               'C',
               cashAccount.OpeningBalance,
                cashAccount.CampusId,
                cashAccount.OpeningBalanceDate
                );

            await _unitOfWork.Ledger.Add(addBalanceInOpeningBalanceEquity);
            await _unitOfWork.SaveAsync();
        }

        public async Task<Response<List<CashAccountDto>>> GetCashAccountDropDown()
        {
            var cashAccounts = await _unitOfWork.CashAccount.GetAll();
            if (!cashAccounts.Any())
                return new Response<List<CashAccountDto>>("List is empty");

            return new Response<List<CashAccountDto>>(_mapper.Map<List<CashAccountDto>>(cashAccounts), "Returning List");
        }
    }
}
