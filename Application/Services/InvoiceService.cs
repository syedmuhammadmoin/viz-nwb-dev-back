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
    public class InvoiceService : IInvoiceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public InvoiceService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<InvoiceDto>> CreateAsync(CreateInvoiceDto entity)
        {
            if (entity.isSubmit)
            {
                return await this.SubmitINV(entity);
            }
            else
            {
                return await this.SaveINV(entity, DocumentStatus.Draft);
            }
        }

        public async Task<PaginationResponse<List<InvoiceDto>>> GetAllAsync(PaginationFilter filter)
        {
            var specification = new InvoiceSpecs(filter);
            var Invs = await _unitOfWork.Invoice.GetAll(specification);

            if (Invs.Count() == 0)
                return new PaginationResponse<List<InvoiceDto>>("List is empty");

            var totalRecords = await _unitOfWork.Invoice.TotalRecord();

            return new PaginationResponse<List<InvoiceDto>>(_mapper.Map<List<InvoiceDto>>(Invs),
                filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<InvoiceDto>> GetByIdAsync(int id)
        {
            var specification = new InvoiceSpecs(false);
            var inv = await _unitOfWork.Invoice.GetById(id, specification);
            if (inv == null)
                return new Response<InvoiceDto>("Not found");

            return new Response<InvoiceDto>(_mapper.Map<InvoiceDto>(inv), "Returning value");
        }

        public async Task<Response<InvoiceDto>> UpdateAsync(CreateInvoiceDto entity)
        {
            if (entity.isSubmit)
            {
                return await this.SubmitINV(entity);
            }
            else
            {
                return await this.UpdateINV(entity, DocumentStatus.Draft);
            }
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }


        //Private Methods for Invoice
        private async Task<Response<InvoiceDto>> SubmitINV(CreateInvoiceDto entity)
        {
            if (entity.Id == null)
            {
                return await this.SaveINV(entity, DocumentStatus.Submitted);
            }
            else
            {
                return await this.UpdateINV(entity, DocumentStatus.Submitted);
            }
        }

        private async Task<Response<InvoiceDto>> SaveINV(CreateInvoiceDto entity, DocumentStatus status)
        {
            if (entity.InvoiceLines.Count() == 0)
                return new Response<InvoiceDto>("Lines are required");

            var inv = _mapper.Map<InvoiceMaster>(entity);

            //Setting status
            inv.setStatus(status);

            _unitOfWork.CreateTransaction();
            try
            {
                //Saving in table
                var result = await _unitOfWork.Invoice.Add(inv);
                await _unitOfWork.SaveAsync();

                //For creating docNo
                inv.CreateDocNo();
                await _unitOfWork.SaveAsync();

                //Adding Invoice to Ledger
                if (status == DocumentStatus.Submitted)
                {
                    await AddToLedger(inv);
                }

                //Commiting the transaction 
                _unitOfWork.Commit();

                //returning response
                return new Response<InvoiceDto>(_mapper.Map<InvoiceDto>(result), "Created successfully");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new Response<InvoiceDto>(ex.Message);
            }
        }

        private async Task<Response<InvoiceDto>> UpdateINV(CreateInvoiceDto entity, DocumentStatus status)
        {
            if (entity.InvoiceLines.Count() == 0)
                return new Response<InvoiceDto>("Lines are required");

            var specification = new InvoiceSpecs(true);
            var inv = await _unitOfWork.Invoice.GetById((int)entity.Id, specification);

            if (inv == null)
                return new Response<InvoiceDto>("Not found");

            if (inv.Status == DocumentStatus.Submitted)
                return new Response<InvoiceDto>("Invoice already submitted");

            inv.setStatus(status);

            _unitOfWork.CreateTransaction();
            try
            {
                //For updating data
                _mapper.Map<CreateInvoiceDto, InvoiceMaster>(entity, inv);

                await _unitOfWork.SaveAsync();

                //Adding Invoice to Ledger
                if (status == DocumentStatus.Submitted)
                {
                    await AddToLedger(inv);
                }

                //Commiting the transaction
                _unitOfWork.Commit();

                //returning response
                return new Response<InvoiceDto>(_mapper.Map<InvoiceDto>(inv), "Created successfully");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new Response<InvoiceDto>(ex.Message);
            }
        }

        private async Task AddToLedger(InvoiceMaster inv)
        {
            var transaction = new Transactions(inv.DocNo, DocType.Invoice);
            await _unitOfWork.Transaction.Add(transaction);
            await _unitOfWork.SaveAsync();

            inv.setTransactionId(transaction.Id);
            await _unitOfWork.SaveAsync();

            //Inserting line amount into recordledger table
            foreach (var line in inv.InvoiceLines)
            {
                var addSalesAmountInRecordLedger = new RecordLedger(
                    transaction.Id,
                    line.AccountId,
                    inv.CustomerId,
                    line.LocationId,
                    line.Description,
                    'C',
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
                        inv.CustomerId,
                        line.LocationId,
                        line.Description,
                        'C',
                        tax
                    );
                    await _unitOfWork.Ledger.Add(addSalesTaxInRecordLedger);
                    await _unitOfWork.SaveAsync();
                }
            }
            var getCustomerAccount = await _unitOfWork.BusinessPartner.GetById(inv.CustomerId);
            var addReceivableInLedger = new RecordLedger(
                        transaction.Id,
                        getCustomerAccount.AccountReceivableId,
                        inv.CustomerId,
                        null,
                        inv.DocNo,
                        'D',
                        inv.TotalAmount
                    );

            await _unitOfWork.Ledger.Add(addReceivableInLedger);
            await _unitOfWork.SaveAsync();
        }
    }
}