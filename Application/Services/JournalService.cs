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
    public class JournalService : IJournalService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public JournalService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<PaginationResponse<List<JournalDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var result = await _unitOfWork.Journals.GetAll(new JournalSpecs(filter, false));
            if (result.Count() == 0)
                return new PaginationResponse<List<JournalDto>>(_mapper.Map<List<JournalDto>>(result), "List is empty");

            var totalRecords = await _unitOfWork.Journals.TotalRecord(new JournalSpecs(filter, true));
            return new PaginationResponse<List<JournalDto>>(_mapper.Map<List<JournalDto>>(result), filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }
        public async Task<Response<JournalDto>> GetByIdAsync(int id)
        {
            var result = await _unitOfWork.Journals.GetById(id, new JournalSpecs());
            if (result == null)
                return new Response<JournalDto>("Not found");

            return new Response<JournalDto>(_mapper.Map<JournalDto>(result), "Returning value");
        }
        public async Task<Response<List<JournalDto>>> GetDropDown()
        {
            var result = await _unitOfWork.Journals.GetAll();
            if (!result.Any())
                return new Response<List<JournalDto>>(null, "List is empty");

            return new Response<List<JournalDto>>(_mapper.Map<List<JournalDto>>(result), "Returning List");
        }

        public async Task<Response<JournalDto>> CreateAsync(CreateJournalDto entity)
        {
            var errors = new List<string>();

            switch (entity.Type)
            {
                case JournalTypes.Sales:
                case JournalTypes.Purchase:
                case JournalTypes.Miscellaneous:
                    if (string.IsNullOrWhiteSpace(entity.DefaultAccountId))
                    {
                        errors.Add("Default Account is required.");
                    }
                  
                 break;

                case JournalTypes.Cash:
                    if (string.IsNullOrWhiteSpace(entity.DefaultAccountId))
                    {
                        errors.Add("Cash Account is required.");
                    }

                    if (string.IsNullOrWhiteSpace(entity.SuspenseAccountId))
                    {
                        errors.Add("Suspense Account is required.");
                    }
                    if (string.IsNullOrWhiteSpace(entity.ProfitAccountId))
                    {
                        errors.Add("Profit Account is required.");
                    }
                    if (string.IsNullOrWhiteSpace(entity.LossAccountId))
                    {
                        errors.Add("Loss Account is required.");
                    }
                    
                    break;
                case JournalTypes.Bank:
                    if (string.IsNullOrWhiteSpace(entity.DefaultAccountId))
                    {
                        errors.Add("Bank Account is required.");
                    }

                    if (string.IsNullOrWhiteSpace(entity.BankAccountNumber))
                    {
                        errors.Add("Bank Number is required.");
                    }
                    if (string.IsNullOrWhiteSpace(entity.SuspenseAccountId))
                    {
                        errors.Add("Suspense Account is required.");
                    }
                    if (string.IsNullOrWhiteSpace(entity.ProfitAccountId))
                    {
                        errors.Add("Profit Account is required.");
                    }
                    if (string.IsNullOrWhiteSpace(entity.LossAccountId))
                    {
                        errors.Add("Loss Account is required.");
                    }
                    
                    break;
                
                default:
                    
                    errors.Add("Invalid JournalType.");
                    break;
            }

            if (errors.Any())
            {
                return new Response<JournalDto>(null, string.Join("; ", errors));
            }
            
         

            if (entity.Type == JournalTypes.Sales || entity.Type == JournalTypes.Purchase || entity.Type == JournalTypes.Miscellaneous)
            {
               
               
                entity.SuspenseAccountId = null;
                entity.ProfitAccountId = null;
                entity.LossAccountId = null;
                entity.BankAccountNumber = null;
            }
            if(entity.Type == JournalTypes.Cash)
            {
                entity.BankAccountNumber = null;
               
            }
           
            var journalEntity = _mapper.Map<Journal>(entity);
            var result = await _unitOfWork.Journals.Add(_mapper.Map<Journal>(journalEntity));
            //var cashAccount = await _unitOfWork.Level4.GetById(entity.CashAccount);
            //var profitAccount = await _unitOfWork.Level4.GetById(entity.ProfitAccount);
            //var suspenseAccount = await _unitOfWork.Level4.GetById(entity.SuspenseAccount);
            //var defaultAccount = await _unitOfWork.Level4.GetById(entity.DefaultAccount);
            //var lossAccount = await _unitOfWork.Level4.GetById(entity.LossAccount);

            await _unitOfWork.SaveAsync();
            return new Response<JournalDto>(_mapper.Map<JournalDto>(result), "Created successfully");
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<JournalDto>> UpdateAsync(CreateJournalDto entity)
        {
            var result = await _unitOfWork.Journals.GetById((int)entity.Id);
            if (result == null)
                return new Response<JournalDto>("Not found");

            //For updating data
            _mapper.Map(entity, result);
            await _unitOfWork.SaveAsync();
            return new Response<JournalDto>(_mapper.Map<JournalDto>(result), "Updated successfully");
        }

        public async Task<Response<bool>> DeleteJournals(List<int> ids)
        {
            if (ids.Count() < 0 || ids == null)
            {
                return new Response<bool>("List Cannot be Empty.");
            }
            else
            {
                foreach (var coa in ids)
                {
                    var Jv = await _unitOfWork.Journals.GetById(coa);
                    if (Jv == null)
                        return new Response<bool>("Journal Not Found.");
                    Jv.IsDelete = true;
                    await _unitOfWork.SaveAsync();
                }
                return new Response<bool>(true, "Deleted Successfully.");
            }
        }
    }
}
