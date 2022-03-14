﻿using Application.Contracts.DTOs;
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
    public class BankAccountService : IBankAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BankAccountService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<BankAccountDto>> CreateAsync(CreateBankAccountDto entity)
        {
            _unitOfWork.CreateTransaction();
            try
            {
                var ChAccount = new Level4(
                    entity.BankName,
                    new Guid("12200000-5566-7788-99AA-BBCCDDEEFF00"),
                    new Guid("10000000-5566-7788-99AA-BBCCDDEEFF00"));

                await _unitOfWork.Level4.Add(ChAccount);

                var ClAccount = new Level4(
                    $"{entity.BankName} Clearing Account",
                    new Guid("12200000-5566-7788-99AA-BBCCDDEEFF00"),
                    new Guid("10000000-5566-7788-99AA-BBCCDDEEFF00"));

                await _unitOfWork.Level4.Add(ClAccount);

                var transaction = new Transactions("1", DocType.BankAccount);
                await _unitOfWork.Transaction.Add(transaction);
                await _unitOfWork.SaveAsync();

                var bankAccount = _mapper.Map<BankAccount>(entity);

                bankAccount.setChAccountId(ChAccount.Id);
                bankAccount.setClAccountId(ChAccount.Id);
                bankAccount.setTransactionId(transaction.Id);

                await _unitOfWork.BankAccount.Add(bankAccount);
                await _unitOfWork.SaveAsync();

                bankAccount.createDocNo();
                transaction.updateDocNo(bankAccount.DocNo);
                await _unitOfWork.SaveAsync();

                //Adding BankAccount to Ledger
                await AddToLedger(bankAccount);

                //Commiting the transaction 
                _unitOfWork.Commit();

                //returning response
                return new Response<BankAccountDto>(_mapper.Map<BankAccountDto>(bankAccount), "Created successfully");

            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new Response<BankAccountDto>(ex.Message);
            }
        }

        public async Task<PaginationResponse<List<BankAccountDto>>> GetAllAsync(PaginationFilter filter)
        {
            var specification = new BankAccountSpecs(filter);
            var backAccount = await _unitOfWork.BankAccount.GetAll(specification);

            if (!backAccount.Any())
                return new PaginationResponse<List<BankAccountDto>>("List is empty");

            var totalRecords = await _unitOfWork.BankAccount.TotalRecord();

            return new PaginationResponse<List<BankAccountDto>>(_mapper.Map<List<BankAccountDto>>(backAccount), filter.PageStart, filter.PageEnd, totalRecords, "Returing list");

        }

        public  async Task<Response<BankAccountDto>> GetByIdAsync(int id)
        {
            var backAccount = await _unitOfWork.BankAccount.GetById(id);
            if (backAccount == null)
                return new Response<BankAccountDto>("Not found");

            return new Response<BankAccountDto>(_mapper.Map<BankAccountDto>(backAccount), "Returning value");
        }

        public async Task<Response<BankAccountDto>> UpdateAsync(UpdateBankAccountDto entity)
        {
            var bankAccount = await _unitOfWork.BankAccount.GetById((int)entity.Id);

            if (bankAccount == null)
                return new Response<BankAccountDto>("Not found");

            //For updating data
            _mapper.Map<UpdateBankAccountDto, BankAccount>(entity, bankAccount);
            await _unitOfWork.SaveAsync();
            return new Response<BankAccountDto>(_mapper.Map<BankAccountDto>(bankAccount), "Updated successfully");
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
        
        //AddLedger in Bank Account
        private async Task AddToLedger(BankAccount bankAccount)
        {
            var addBalanceInBankAccount = new RecordLedger(
                bankAccount.TransactionId,
                bankAccount.ChAccountId,
                null,
                null,
                "Opening Balance",
                'D',
                bankAccount.OpeningBalance);

            await _unitOfWork.Ledger.Add(addBalanceInBankAccount);

            var addBalanceInOpeningBalanceEquity = new RecordLedger(
               bankAccount.TransactionId,
               new Guid("31260000-5566-7788-99AA-BBCCDDEEFF00"),
               null,
               null,
               "Opening Balance",
               'C',
               bankAccount.OpeningBalance);

            await _unitOfWork.Ledger.Add(addBalanceInOpeningBalanceEquity);
            await _unitOfWork.SaveAsync();
        }
    }
}