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
            // checking account number already exist or not
            var checkExistingAccount = _unitOfWork.BankAccount.Find(new BankAccountSpecs(entity.AccountNumber)).FirstOrDefault();

            if (checkExistingAccount != null)
                return new Response<BankAccountDto>("Account number already exist");

            var checkingCode = _unitOfWork.Level4.Find(new Level4Specs(entity.AccountCode)).FirstOrDefault();
            if (checkingCode != null)
                return new Response<BankAccountDto>("Duplicate account code");

            var checkingCodeForClearingAccount = _unitOfWork.Level4.Find(new Level4Specs($"{entity.AccountCode}C")).FirstOrDefault();
            if (checkingCodeForClearingAccount != null)
                return new Response<BankAccountDto>("Duplicate account code");

            _unitOfWork.CreateTransaction();
          
                var ChAccount = new Level4(
                    entity.AccountTitle,
                    entity.AccountCode,
                    new Guid("12500000-5566-7788-99AA-BBCCDDEEFF00"),
                    new Guid("10000000-5566-7788-99AA-BBCCDDEEFF00"));

                await _unitOfWork.Level4.Add(ChAccount);

                var ClAccount = new Level4(
                    $"{entity.AccountTitle} Clearing Account",
                    $"{entity.AccountCode}C",
                    new Guid("12500000-5566-7788-99AA-BBCCDDEEFF00"),
                    new Guid("10000000-5566-7788-99AA-BBCCDDEEFF00"));

                await _unitOfWork.Level4.Add(ClAccount);

                var transaction = new Transactions(0, "1", DocType.BankAccount);
                await _unitOfWork.Transaction.Add(transaction);
                await _unitOfWork.SaveAsync();

                var bankAccount = _mapper.Map<BankAccount>(entity);

                bankAccount.setChAccountId(ChAccount.Id);
                bankAccount.setClAccountId(ClAccount.Id);
                bankAccount.setTransactionId(transaction.Id);

                await _unitOfWork.BankAccount.Add(bankAccount);
                await _unitOfWork.SaveAsync();

                bankAccount.createDocNo();
                transaction.updateDocNo(bankAccount.Id, bankAccount.DocNo);
                await _unitOfWork.SaveAsync();

                //Adding BankAccount to Ledger
                await AddToLedger(bankAccount);

                //Commiting the transaction 
                _unitOfWork.Commit();

                //returning response
                return new Response<BankAccountDto>(_mapper.Map<BankAccountDto>(bankAccount), "Created successfully");

       
        }

        public async Task<PaginationResponse<List<BankAccountDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var backAccount = await _unitOfWork.BankAccount.GetAll(new BankAccountSpecs(filter, false));

            if (!backAccount.Any())
                return new PaginationResponse<List<BankAccountDto>>(_mapper.Map<List<BankAccountDto>>(backAccount), "List is empty");

            var totalRecords = await _unitOfWork.BankAccount.TotalRecord(new BankAccountSpecs(filter, true));

            return new PaginationResponse<List<BankAccountDto>>(_mapper.Map<List<BankAccountDto>>(backAccount), filter.PageStart, filter.PageEnd, totalRecords, "Returing list");

        }

        public async Task<Response<BankAccountDto>> GetByIdAsync(int id)
        {
            var specification = new BankAccountSpecs();
            var backAccount = await _unitOfWork.BankAccount.GetById(id, specification);
            if (backAccount == null)
                return new Response<BankAccountDto>("Not found");

            return new Response<BankAccountDto>(_mapper.Map<BankAccountDto>(backAccount), "Returning value");
        }

        public async Task<Response<BankAccountDto>> UpdateAsync(UpdateBankAccountDto entity)
        {
            var bankAccount = await _unitOfWork.BankAccount.GetById((int)entity.Id);
            if (bankAccount == null)
                return new Response<BankAccountDto>("Not found");

            var checkingCode = _unitOfWork.Level4.Find(new Level4Specs(entity.AccountCode, bankAccount.ChAccountId)).FirstOrDefault();
            if (checkingCode != null)
                return new Response<BankAccountDto>("Duplicate code");

            var checkingCodeForClearingAccount = _unitOfWork.Level4
                .Find(new Level4Specs($"{entity.AccountCode}C", bankAccount.ClearingAccountId)).FirstOrDefault();
            if (checkingCodeForClearingAccount != null)
                return new Response<BankAccountDto>("Duplicate code");

            _unitOfWork.CreateTransaction();
           
                //For updating data
                _mapper.Map<UpdateBankAccountDto, BankAccount>(entity, bankAccount);

                // Getting account detail in COA
                var account = await _unitOfWork.Level4.GetById(bankAccount.ChAccountId);
                if (account == null)
                    return new Response<BankAccountDto>("Account not found in Chart Of Account");

                //Updating account name in chart of account
                account.setAccountName(entity.AccountTitle, entity.AccountCode);

                // Getting clearing account detail in COA
                var clearingAccount = await _unitOfWork.Level4.GetById(bankAccount.ClearingAccountId);
                if (clearingAccount == null)
                    return new Response<BankAccountDto>("Clearing account not found in Chart Of Account");

                //Updating clearing account name in chart of account
                clearingAccount.setAccountName($"{entity.AccountTitle} Clearing Account", $"{entity.AccountCode}C");

                await _unitOfWork.SaveAsync();
                _unitOfWork.Commit();
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
                bankAccount.OpeningBalance,
                bankAccount.CampusId,
                bankAccount.OpeningBalanceDate
                );

            await _unitOfWork.Ledger.Add(addBalanceInBankAccount);

            var addBalanceInOpeningBalanceEquity = new RecordLedger(
               bankAccount.TransactionId,
               new Guid("32110000-5566-7788-99AA-BBCCDDEEFF00"),
               null,
               null,
               "Opening Balance",
               'C',
               bankAccount.OpeningBalance,
               bankAccount.CampusId,
               bankAccount.OpeningBalanceDate
                );

            await _unitOfWork.Ledger.Add(addBalanceInOpeningBalanceEquity);
            await _unitOfWork.SaveAsync();
        }

        public async Task<Response<List<BankAccountDto>>> GetBankAccountDropDown()
        {
            var bankAccounts = await _unitOfWork.BankAccount.GetAll();
            if (!bankAccounts.Any())
                return new Response<List<BankAccountDto>>("List is empty");

            return new Response<List<BankAccountDto>>(_mapper.Map<List<BankAccountDto>>(bankAccounts), "Returning List");
        }
    }
}
