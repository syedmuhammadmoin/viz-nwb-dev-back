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
    public class CreditNoteService : ICreditNoteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreditNoteService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Response<CreditNoteDto>> CreateAsync(CreateCreditNoteDto entity)
        {
            if (entity.isSubmit)
            {
                return await this.SubmitCRN(entity);
            }
            else
            {
                return await this.SaveCRN(entity, 1);
            }
        }

        public async Task<PaginationResponse<List<CreditNoteDto>>> GetAllAsync(PaginationFilter filter)
        {
            var specification = new CreditNoteSpecs(filter);
            var crns = await _unitOfWork.CreditNote.GetAll(specification);

            if (crns.Count() == 0)
                return new PaginationResponse<List<CreditNoteDto>>("List is empty");

            var totalRecords = await _unitOfWork.CreditNote.TotalRecord();

            return new PaginationResponse<List<CreditNoteDto>>(_mapper.Map<List<CreditNoteDto>>(crns),
                filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<CreditNoteDto>> GetByIdAsync(int id)
        {
            var specification = new CreditNoteSpecs(false);
            var crn = await _unitOfWork.CreditNote.GetById(id, specification);
            if (crn == null)
                return new Response<CreditNoteDto>("Not found");

            return new Response<CreditNoteDto>(_mapper.Map<CreditNoteDto>(crn), "Returning value");
        }

        public async Task<Response<CreditNoteDto>> UpdateAsync(CreateCreditNoteDto entity)
        {
            if (entity.isSubmit)
            {
                return await this.SubmitCRN(entity);
            }
            else
            {
                return await this.UpdateCRN(entity, 1);
            }
        }
        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        //Private methods for CreditNote
        private async Task<Response<CreditNoteDto>> SubmitCRN(CreateCreditNoteDto entity)
        {
            if (entity.Id == null)
            {
                return await this.SaveCRN(entity, 6);
            }
            else
            {
                return await this.UpdateCRN(entity, 6);
            }
        }

        private async Task<Response<CreditNoteDto>> SaveCRN(CreateCreditNoteDto entity, int status)
        {
            if (entity.CreditNoteLines.Count() == 0)
                return new Response<CreditNoteDto>("Lines are required");

            var crn = _mapper.Map<CreditNoteMaster>(entity);
            
            //setting BusinessPartnerReceivable
            var er = await _unitOfWork.BusinessPartner.GetById(entity.CustomerId);
            crn.setReceivableAccount(er.AccountReceivableId);

            //Setting status
            crn.setStatus(status);

            _unitOfWork.CreateTransaction();
            try
            {
                //Saving in table
                var result = await _unitOfWork.CreditNote.Add(crn);
                await _unitOfWork.SaveAsync();

                //For creating docNo
                crn.CreateDocNo();
                await _unitOfWork.SaveAsync();

                //Commiting the transaction 
                _unitOfWork.Commit();

                //returning response
                return new Response<CreditNoteDto>(_mapper.Map<CreditNoteDto>(result), "Created successfully");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new Response<CreditNoteDto>(ex.Message);
            }
        }

        private async Task<Response<CreditNoteDto>> UpdateCRN(CreateCreditNoteDto entity, int status)
        {
            if (entity.CreditNoteLines.Count() == 0)
                return new Response<CreditNoteDto>("Lines are required");

            var specification = new CreditNoteSpecs(true);
            var crn = await _unitOfWork.CreditNote.GetById((int)entity.Id, specification);

            if (crn == null)
                return new Response<CreditNoteDto>("Not found");

            if (crn.StatusId != 1 && crn.StatusId != 2)
                return new Response<CreditNoteDto>("Only draft document can be edited");

            //setting BusinessPartnerReceivable
            var er = await _unitOfWork.BusinessPartner.GetById(entity.CustomerId);
            crn.setReceivableAccount(er.AccountReceivableId);
            
            crn.setStatus(status);

            _unitOfWork.CreateTransaction();
            try
            {
                //For updating data
                _mapper.Map<CreateCreditNoteDto, CreditNoteMaster>(entity, crn);

                await _unitOfWork.SaveAsync();

                //Commiting the transaction
                _unitOfWork.Commit();

                //returning response
                return new Response<CreditNoteDto>(_mapper.Map<CreditNoteDto>(crn), "Created successfully");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new Response<CreditNoteDto>(ex.Message);
            }
        }

        private async Task AddToLedger(CreditNoteMaster crn)
        {
            var transaction = new Transactions(crn.DocNo, DocType.CreditNote);
            await _unitOfWork.Transaction.Add(transaction);
            await _unitOfWork.SaveAsync();

            crn.setTransactionId(transaction.Id);
            await _unitOfWork.SaveAsync();

            //Inserting line amount into recordledger table
            foreach (var line in crn.CreditNoteLines)
            {
                var addSalesAmountInRecordLedger = new RecordLedger(
                    transaction.Id,
                    line.AccountId,
                    crn.CustomerId,
                    line.WarehouseId,
                    line.Description,
                    'D',
                    line.Price * line.Quantity
                    );

                await _unitOfWork.Ledger.Add(addSalesAmountInRecordLedger);
                await _unitOfWork.SaveAsync();

                var tax = (line.Quantity * line.Price * line.Tax) / 100;

                if (tax > 0)
                {
                    var addSalesTaxInRecordLedger = new RecordLedger(
                        transaction.Id,
                        line.AccountId,
                        crn.CustomerId,
                        line.WarehouseId,
                        line.Description,
                        'D',
                        tax
                    );
                    await _unitOfWork.Ledger.Add(addSalesTaxInRecordLedger);
                    await _unitOfWork.SaveAsync();
                }
            }
            var getCustomerAccount = await _unitOfWork.BusinessPartner.GetById(crn.CustomerId);
            var addReceivableInLedger = new RecordLedger(
                        transaction.Id,
                        getCustomerAccount.AccountReceivableId,
                        crn.CustomerId,
                        null,
                        crn.DocNo,
                        'C',
                        crn.TotalAmount
                    );

            await _unitOfWork.Ledger.Add(addReceivableInLedger);
            await _unitOfWork.SaveAsync();
        }

    }
}
