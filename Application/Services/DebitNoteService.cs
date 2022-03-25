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
    public class DebitNoteService : IDebitNoteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DebitNoteService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<DebitNoteDto>> CreateAsync(CreateDebitNoteDto entity)
        {
            if (entity.isSubmit)
            {
                return await this.SubmitDBN(entity);
            }
            else
            {
                return await this.SaveDBN(entity, 1);
            }
        }

        public async Task<PaginationResponse<List<DebitNoteDto>>> GetAllAsync(PaginationFilter filter)
        {
            var specification = new DebitNoteSpecs(filter);
            var dbns = await _unitOfWork.DebitNote.GetAll(specification);

            if (dbns.Count() == 0)
                return new PaginationResponse<List<DebitNoteDto>>("List is empty");

            var totalRecords = await _unitOfWork.DebitNote.TotalRecord();

            return new PaginationResponse<List<DebitNoteDto>>(_mapper.Map<List<DebitNoteDto>>(dbns),
                filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<DebitNoteDto>> GetByIdAsync(int id)
        {
            var specification = new DebitNoteSpecs(false);
            var dbn = await _unitOfWork.DebitNote.GetById(id, specification);
            if (dbn == null)
                return new Response<DebitNoteDto>("Not found");

            return new Response<DebitNoteDto>(_mapper.Map<DebitNoteDto>(dbn), "Returning value");
        }

        public async Task<Response<DebitNoteDto>> UpdateAsync(CreateDebitNoteDto entity)
        {
            if (entity.isSubmit)
            {
                return await this.SubmitDBN(entity);
            }
            else
            {
                return await this.UpdateDBN(entity, 1);
            }
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        //Privte Methods for DebitNote

        private async Task<Response<DebitNoteDto>> SubmitDBN(CreateDebitNoteDto entity)
        {
            if (entity.Id == null)
            {
                return await this.SaveDBN(entity, 6);
            }
            else
            {
                return await this.UpdateDBN(entity, 6);
            }
        }

        private async Task<Response<DebitNoteDto>> SaveDBN(CreateDebitNoteDto entity, int status)
        {
            if (entity.DebitNoteLines.Count() == 0)
                return new Response<DebitNoteDto>("Lines are required");

            var dbn = _mapper.Map<DebitNoteMaster>(entity);

            //Setting status
            dbn.setStatus(status);

            _unitOfWork.CreateTransaction();
            try
            {
                //Saving in table
                var result = await _unitOfWork.DebitNote.Add(dbn);
                await _unitOfWork.SaveAsync();

                //For creating docNo
                dbn.CreateDocNo();
                await _unitOfWork.SaveAsync();

                //Commiting the transaction 
                _unitOfWork.Commit();

                //returning response
                return new Response<DebitNoteDto>(_mapper.Map<DebitNoteDto>(result), "Created successfully");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new Response<DebitNoteDto>(ex.Message);
            }
        }

        private async Task<Response<DebitNoteDto>> UpdateDBN(CreateDebitNoteDto entity, int status)
        {
            if (entity.DebitNoteLines.Count() == 0)
                return new Response<DebitNoteDto>("Lines are required");

            var specification = new DebitNoteSpecs(true);
            var dbn = await _unitOfWork.DebitNote.GetById((int)entity.Id, specification);

            if (dbn == null)
                return new Response<DebitNoteDto>("Not found");

            if (dbn.StatusId != 1 && dbn.StatusId != 2)
                return new Response<DebitNoteDto>("Only draft document can be edited");

            dbn.setStatus(status);

            _unitOfWork.CreateTransaction();
            try
            {
                //For updating data
                _mapper.Map<CreateDebitNoteDto, DebitNoteMaster>(entity, dbn);

                await _unitOfWork.SaveAsync();

                //Commiting the transaction
                _unitOfWork.Commit();

                //returning response
                return new Response<DebitNoteDto>(_mapper.Map<DebitNoteDto>(dbn), "Created successfully");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new Response<DebitNoteDto>(ex.Message);
            }
        }

        private async Task AddToLedger(DebitNoteMaster dbn)
        {
            var transaction = new Transactions(dbn.DocNo, DocType.DebitNote);
            await _unitOfWork.Transaction.Add(transaction);
            await _unitOfWork.SaveAsync();

            dbn.setTransactionId(transaction.Id);
            await _unitOfWork.SaveAsync();

            //Inserting line amount into recordledger table
            foreach (var line in dbn.DebitNoteLines)
            {
                var tax = (line.Quantity * line.Cost * line.Tax) / 100;
                var amount = line.Quantity * line.Cost;

                var addSalesAmountInRecordLedger = new RecordLedger(
                    transaction.Id,
                    line.AccountId,
                    dbn.VendorId,
                    line.WarehouseId,
                    line.Description,
                    'C',
                    amount + tax
                    );

                await _unitOfWork.Ledger.Add(addSalesAmountInRecordLedger);
                await _unitOfWork.SaveAsync();
            }
            var getVendorAccount = await _unitOfWork.BusinessPartner.GetById(dbn.VendorId);
            var addPayableInLedger = new RecordLedger(
                        transaction.Id,
                        getVendorAccount.AccountPayableId,
                        dbn.VendorId,
                        null,
                        dbn.DocNo,
                        'D',
                        dbn.TotalAmount
                    );

            await _unitOfWork.Ledger.Add(addPayableInLedger);
            await _unitOfWork.SaveAsync();
        }
    }
}
