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
    public class CashAccountService : ICashAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CashAccountService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<CashAccountDto>> CreateAsync(CreateCashAccountDto entity)
        {
            _unitOfWork.CreateTransaction();
            try
            {
                var ChAccount = new Level4(
                    entity.CashAccountName,
                    new Guid("12100000-5566-7788-99AA-BBCCDDEEFF00"),
                    new Guid("10000000-5566-7788-99AA-BBCCDDEEFF00"));

                await _unitOfWork.Level4.Add(ChAccount);

                var transaction = new Transactions(0, "1", DocType.CashAccount);
                await _unitOfWork.Transaction.Add(transaction);
                await _unitOfWork.SaveAsync();

                //Inserting into cashAccount table
                var cashAccount = _mapper.Map<CashAccount>(entity);

                cashAccount.setChAccountId(ChAccount.Id);
                cashAccount.setTransactionId(transaction.Id);

                await _unitOfWork.CashAccount.Add(cashAccount);
                await _unitOfWork.SaveAsync();

                cashAccount.createDocNo();
                transaction.updateDocNo(cashAccount.Id, cashAccount.DocNo);
                await _unitOfWork.SaveAsync();

                //Adding CashAccount to Ledger
                await AddToLedger(cashAccount);

                //Commiting the transaction 
                _unitOfWork.Commit();

                //returning response
                return new Response<CashAccountDto>(_mapper.Map<CashAccountDto>(cashAccount), "Created successfully");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new Response<CashAccountDto>(ex.Message);
            }
        }

        public async Task<PaginationResponse<List<CashAccountDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var specification = new CashAccountSpecs(filter);
            var cashAccount = await _unitOfWork.CashAccount.GetAll(specification);

            if (!cashAccount.Any())
                return new PaginationResponse<List<CashAccountDto>>(_mapper.Map<List<CashAccountDto>>(cashAccount), "List is empty");

            var totalRecords = await _unitOfWork.CashAccount.TotalRecord(specification);

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
            _unitOfWork.CreateTransaction();
            try
            {

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
                account.setAccountName(entity.CashAccountName);


                await _unitOfWork.SaveAsync();
                _unitOfWork.Commit();
                return new Response<CashAccountDto>(_mapper.Map<CashAccountDto>(cashAccount), "Updated successfully");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new Response<CashAccountDto>(ex.Message);
            }
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
               new Guid("31260000-5566-7788-99AA-BBCCDDEEFF00"),
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
