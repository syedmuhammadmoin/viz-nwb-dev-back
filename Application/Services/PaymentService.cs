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
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PaymentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<PaymentDto>> CreateAsync(CreatePaymentDto entity)
        {
            if (entity.isSubmit)
            {
                return await this.SubmitPay(entity);
            }
            else
            {
                return await this.SavePay(entity, DocumentStatus.Draft);
            }
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PaginationResponse<List<PaymentDto>>> GetAllAsync(PaginationFilter filter)
        {
            var specification = new PaymentSpecs(filter);
            var payment = await _unitOfWork.Payment.GetAll(specification);

            if (payment.Count() == 0)
                return new PaginationResponse<List<PaymentDto>>("List is empty");

            var totalRecords = await _unitOfWork.Payment.TotalRecord();

            return new PaginationResponse<List<PaymentDto>>(_mapper.Map<List<PaymentDto>>(payment), filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<PaymentDto>> GetByIdAsync(int id)
        {
            var specification = new PaymentSpecs();
            var payment = await _unitOfWork.Payment.GetById(id, specification);
            if (payment == null)
                return new Response<PaymentDto>("Not found");

            return new Response<PaymentDto>(_mapper.Map<PaymentDto>(payment), "Returning value");
        }

        public async Task<Response<PaymentDto>> UpdateAsync(CreatePaymentDto entity)
        {
            if (entity.isSubmit)
            {
                return await this.SubmitPay(entity);
            }
            else
            {
                return await this.UpdatePay(entity, DocumentStatus.Draft);
            }
        }


        private async Task<Response<PaymentDto>> SubmitPay(CreatePaymentDto entity)
        {
            if (entity.Id == null)
            {
                return await this.SavePay(entity, DocumentStatus.Submitted);
            }
            else
            {
                return await this.UpdatePay(entity, DocumentStatus.Submitted);
            }
        }

        private async Task<Response<PaymentDto>> SavePay(CreatePaymentDto entity, DocumentStatus status)
        {
            var payment = _mapper.Map<Payment>(entity);

            //Setting status
            payment.setStatus(status);

            _unitOfWork.CreateTransaction();
            try
            {
                //Saving in table
                var result = await _unitOfWork.Payment.Add(payment);
                await _unitOfWork.SaveAsync();

                //For creating docNo
                payment.CreateDocNo();
                await _unitOfWork.SaveAsync();

                //Adding Invoice to Ledger
                if (status == DocumentStatus.Submitted)
                {
                    await AddToLedger(payment);
                }

                //Commiting the transaction 
                _unitOfWork.Commit();

                //returning response
                return new Response<PaymentDto>(_mapper.Map<PaymentDto>(result), "Created successfully");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new Response<PaymentDto>(ex.Message);
            }
        }

        private async Task<Response<PaymentDto>> UpdatePay(CreatePaymentDto entity, DocumentStatus status)
        {
            var payment = await _unitOfWork.Payment.GetById((int)entity.Id);

            if (payment == null)
                return new Response<PaymentDto>("Not found");

            if (payment.Status == DocumentStatus.Submitted)
                return new Response<PaymentDto>("Payment already submitted");

            payment.setStatus(status);

            _unitOfWork.CreateTransaction();
            try
            {
                //For updating data
                _mapper.Map<CreatePaymentDto, Payment>(entity, payment);

                await _unitOfWork.SaveAsync();

                //Adding Invoice to Ledger
                if (status == DocumentStatus.Submitted)
                {
                    await AddToLedger(payment);
                }

                //Commiting the transaction
                _unitOfWork.Commit();

                //returning response
                return new Response<PaymentDto>(_mapper.Map<PaymentDto>(payment), "Updated successfully");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new Response<PaymentDto>(ex.Message);
            }
        }

        private async Task AddToLedger(Payment payment)
        {
            var transaction = new Transactions(payment.DocNo, DocType.Payment);
            await _unitOfWork.Transaction.Add(transaction);
            await _unitOfWork.SaveAsync();

            payment.setTransactionId(transaction.Id);
            await _unitOfWork.SaveAsync();

            if (payment.PaymentType == PaymentType.Inflow)
            {
                var addGrossAmountInRecordLedger = new RecordLedger(
                    transaction.Id,
                    payment.AccountId,
                    payment.BusinessPartnerId,
                    null,
                    payment.Description,
                    'C',
                    payment.GrossPayment
                    );

                await _unitOfWork.Ledger.Add(addGrossAmountInRecordLedger);

                if (payment.Discount > 0)
                {
                    var addDiscountInRecordLedger = new RecordLedger(
                        transaction.Id,
                    payment.AccountId,
                    payment.BusinessPartnerId,
                    null,
                    payment.Description,
                    'D',
                    payment.Discount
                        );

                    await _unitOfWork.Ledger.Add(addDiscountInRecordLedger); 
                }

                if (payment.SalesTax > 0)
                {
                    var addSalesTaxInRecordLedger = new RecordLedger(

                    transaction.Id,
                    payment.AccountId,
                    payment.BusinessPartnerId,
                    null,
                    payment.Description,
                    'D',
                    payment.SalesTax
                        );
                    await _unitOfWork.Ledger.Add(addSalesTaxInRecordLedger);
                }

                if (payment.IncomeTax > 0)
                {
                    var addIncomeTaxInRecordLedger = new RecordLedger(

                        transaction.Id,
                    payment.AccountId,
                    payment.BusinessPartnerId,
                    null,
                    payment.Description,
                    'D',
                    payment.IncomeTax);
                    await _unitOfWork.Ledger.Add(addIncomeTaxInRecordLedger);
                }
                var addNetPaymentInRecordLedger = new RecordLedger(
                    transaction.Id,
                    payment.AccountId,
                    payment.BusinessPartnerId,
                    null,
                    payment.Description,
                    'D',
                    (payment.GrossPayment - payment.Discount - payment.SalesTax - payment.IncomeTax));
                
                await _unitOfWork.Ledger.Add(addNetPaymentInRecordLedger);
            }

            if (payment.PaymentType == PaymentType.Outflow)
            {
                var addGrossAmountInRecordLedger = new RecordLedger(
                    transaction.Id,
                    payment.AccountId,
                    payment.BusinessPartnerId,
                    null,
                    payment.Description,
                    'D',
                    payment.GrossPayment
                    );

                await _unitOfWork.Ledger.Add(addGrossAmountInRecordLedger);

                if (payment.Discount > 0)
                {
                    var addDiscountInRecordLedger = new RecordLedger(
                    transaction.Id,
                    payment.AccountId,
                    payment.BusinessPartnerId,
                    null,
                    payment.Description,
                    'C',
                    payment.Discount
                        );

                    await _unitOfWork.Ledger.Add(addDiscountInRecordLedger);
                }

                if (payment.SalesTax > 0)
                {
                    var addSalesTaxInRecordLedger = new RecordLedger(

                    transaction.Id,
                    payment.AccountId,
                    payment.BusinessPartnerId,
                    null,
                    payment.Description,
                    'C',
                    payment.SalesTax
                        );
                    await _unitOfWork.Ledger.Add(addSalesTaxInRecordLedger);
                }

                if (payment.IncomeTax > 0)
                {
                    var addIncomeTaxInRecordLedger = new RecordLedger(

                    transaction.Id,
                    payment.AccountId,
                    payment.BusinessPartnerId,
                    null,
                    payment.Description,
                    'C',
                    payment.IncomeTax);
                    await _unitOfWork.Ledger.Add(addIncomeTaxInRecordLedger);
                }
                var addNetPaymentInRecordLedger = new RecordLedger(
                    transaction.Id,
                    payment.AccountId,
                    payment.BusinessPartnerId,
                    null,
                    payment.Description,
                    'C',
                    (payment.GrossPayment - payment.Discount - payment.SalesTax - payment.IncomeTax));

                await _unitOfWork.Ledger.Add(addNetPaymentInRecordLedger);
            }

            await _unitOfWork.SaveAsync();
        }
    }
}
