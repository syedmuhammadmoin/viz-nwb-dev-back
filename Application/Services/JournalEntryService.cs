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
    public class JournalEntryService : IJournalEntryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public JournalEntryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<JournalEntryDto>> CreateAsync(CreateJournalEntryDto entity)
        {
            if (entity.isSubmit)
            {
                return await this.SubmitJV(entity);
            }
            else
            {
                return await this.SaveJV(entity, DocumentStatus.Draft);
            }
        }

        public async Task<Response<JournalEntryDto>> UpdateAsync(CreateJournalEntryDto entity)
        {
            if (entity.isSubmit)
            {
                return await this.SubmitJV(entity);
            }
            else
            {
                return await this.UpdateJV(entity, DocumentStatus.Draft);
            }
        }

        public async Task<PaginationResponse<List<JournalEntryDto>>> GetAllAsync(PaginationFilter filter)
        {
            var specification = new JournalEntrySpecs(filter);
            var jvs = await _unitOfWork.JournalEntry.GetAll(specification);

            if (jvs.Count() == 0)
                return new PaginationResponse<List<JournalEntryDto>>("List is empty");

            var totalRecords = await _unitOfWork.JournalEntry.TotalRecord();

            return new PaginationResponse<List<JournalEntryDto>>(_mapper.Map<List<JournalEntryDto>>(jvs),
                filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<JournalEntryDto>> GetByIdAsync(int id)
        {
            var specification = new JournalEntrySpecs();
            var jv = await _unitOfWork.JournalEntry.GetById(id, specification);
            if (jv == null)
                return new Response<JournalEntryDto>("Not found");

            return new Response<JournalEntryDto>(_mapper.Map<JournalEntryDto>(jv), "Returning value");
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        //Private Methods for JournalEntry
        private async Task<Response<JournalEntryDto>> SubmitJV(CreateJournalEntryDto entity)
        {
            if (entity.Id == null)
            {
                return await this.SaveJV(entity, DocumentStatus.Submitted);
            }
            else
            {
                return await this.UpdateJV(entity, DocumentStatus.Submitted);
            }
        }

        private async Task<Response<JournalEntryDto>> SaveJV(CreateJournalEntryDto entity, DocumentStatus status)
        {
            if (entity.JournalEntryLines.Count() == 0)
                return new Response<JournalEntryDto>("Lines are required");

            var jv = _mapper.Map<JournalEntryMaster>(entity);

            if (jv.TotalDebit != jv.TotalCredit)
                return new Response<JournalEntryDto>("Sum of debit and credit must be equal");

            //Setting status
            jv.setStatus(status);

            _unitOfWork.CreateTransaction();
            try
            {
                //Saving in table
                var result = await _unitOfWork.JournalEntry.Add(jv);
                await _unitOfWork.SaveAsync();

                //For creating docNo
                jv.CreateDocNo();
                await _unitOfWork.SaveAsync();

                if (status == DocumentStatus.Submitted)
                {
                    await AddToLedger(jv);
                }

                //Commiting the transaction
                _unitOfWork.Commit();

                //returning response
                return new Response<JournalEntryDto>(_mapper.Map<JournalEntryDto>(result), "Created successfully");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new Response<JournalEntryDto>(ex.Message);
            }
        }

        private async Task<Response<JournalEntryDto>> UpdateJV(CreateJournalEntryDto entity, DocumentStatus status)
        {
            if (entity.JournalEntryLines.Count() == 0)
                return new Response<JournalEntryDto>("Lines are required");

            var totalDebit = entity.JournalEntryLines.Sum(i => i.Debit);
            var totalCredit = entity.JournalEntryLines.Sum(i => i.Credit);

            if (totalDebit != totalCredit)
                return new Response<JournalEntryDto>("Sum of debit and credit must be equal");

            var specification = new JournalEntrySpecs();
            var jv = await _unitOfWork.JournalEntry.GetById((int)entity.Id, specification);

            if (jv == null)
                return new Response<JournalEntryDto>("Not found");

            if (jv.Status == DocumentStatus.Submitted)
                return new Response<JournalEntryDto>("Journal voucher already submitted");

            jv.setStatus(status);

            _unitOfWork.CreateTransaction();
            try
            {
                //For updating data
                _mapper.Map<CreateJournalEntryDto, JournalEntryMaster>(entity, jv);

                await _unitOfWork.SaveAsync();

                if (status == DocumentStatus.Submitted)
                {
                    await AddToLedger(jv);
                }

                //Commiting the transaction
                _unitOfWork.Commit();

                //returning response
                return new Response<JournalEntryDto>(_mapper.Map<JournalEntryDto>(jv), "Created successfully");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new Response<JournalEntryDto>(ex.Message);
            }
        }

        private async Task AddToLedger(JournalEntryMaster jv)
        {
            var transaction = new Transactions(jv.DocNo, DocType.JournalEntry);
            var addTransaction = await _unitOfWork.Transaction.Add(transaction);
            await _unitOfWork.SaveAsync();

            jv.setTrasactionId(transaction.Id);
            await _unitOfWork.SaveAsync();

            //Inserting data into recordledger table
            List<RecordLedger> recordLedger = jv.JournalEntryLines
                .Select(line => new RecordLedger(
                    transaction.Id,
                    line.AccountId,
                    line.BusinessPartnerId,
                    line.LocationId,
                    line.Description,
                    line.Debit > 0 && line.Credit <= 0 ? 'D' : 'C',
                    line.Debit > 0 && line.Credit <= 0 ? line.Debit : line.Credit
                    )).ToList();

            await _unitOfWork.Ledger.AddRange(recordLedger);
            await _unitOfWork.SaveAsync();
        }
    }
}
