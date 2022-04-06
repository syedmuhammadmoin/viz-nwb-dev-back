using Application.Contracts.DTOs;
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
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PaymentService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Response<PaymentDto>> CreateAsync(CreatePaymentDto entity)
        {
            if (entity.isSubmit)
            {
                return await this.SubmitPay(entity);
            }
            else
            {
                return await this.SavePay(entity, 1);
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

            var paymentDto = _mapper.Map<PaymentDto>(payment);

            paymentDto.IsAllowedRole = false;
            var workflow = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.Payment)).FirstOrDefault();


            if (workflow != null)
            {
                var transition = workflow.WorkflowTransitions
                    .FirstOrDefault(x => (x.CurrentStatusId == paymentDto.StatusId));

                if (transition != null)
                {
                    var currentUserRoles = new GetUser(this._httpContextAccessor).GetCurrentUserRoles();
                    foreach (var role in currentUserRoles)
                    {
                        if (transition.AllowedRole.Name == role)
                        {
                            paymentDto.IsAllowedRole = true;
                        }
                    }
                }
            }
            return new Response<PaymentDto>(paymentDto, "Returning value");
        }

        public async Task<Response<PaymentDto>> UpdateAsync(CreatePaymentDto entity)
        {
            if (entity.isSubmit)
            {
                return await this.SubmitPay(entity);
            }
            else
            {
                return await this.UpdatePay(entity, 1);
            }
        }

        private async Task<Response<PaymentDto>> SubmitPay(CreatePaymentDto entity)
        {
            var checkingActiveWorkFlows = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.Payment)).FirstOrDefault();

            if (checkingActiveWorkFlows == null)
            {
                return new Response<PaymentDto>("No workflow found for this document");
            }

            if (entity.Id == null)
            {
                return await this.SavePay(entity, 6);
            }
            else
            {
                return await this.UpdatePay(entity, 6);
            }
        }

        private async Task<Response<PaymentDto>> SavePay(CreatePaymentDto entity, int status)
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

        private async Task<Response<PaymentDto>> UpdatePay(CreatePaymentDto entity, int status)
        {
            var payment = await _unitOfWork.Payment.GetById((int)entity.Id);

            if (payment == null)
                return new Response<PaymentDto>("Not found");

            if (payment.StatusId != 1 && payment.StatusId != 2)
                return new Response<PaymentDto>("Only draft payments can be edited");

            payment.setStatus(status);

            _unitOfWork.CreateTransaction();
            try
            {
                //For updating data
                _mapper.Map<CreatePaymentDto, Payment>(entity, payment);

                //saving into database
                await _unitOfWork.SaveAsync();

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
                    payment.GrossPayment,
                    payment.CampusId,
                    payment.PaymentDate
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
                    payment.Discount,
                    payment.CampusId,
                    payment.PaymentDate
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
                    payment.SalesTax,
                    payment.CampusId,
                    payment.PaymentDate
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
                    payment.IncomeTax,
                    payment.CampusId,
                    payment.PaymentDate);
                    await _unitOfWork.Ledger.Add(addIncomeTaxInRecordLedger);
                }
                var addNetPaymentInRecordLedger = new RecordLedger(
                    transaction.Id,
                    payment.PaymentRegisterId,
                    payment.BusinessPartnerId,
                    null,
                    payment.Description,
                    'D',
                    (payment.GrossPayment - payment.Discount - payment.SalesTax - payment.IncomeTax),
                    payment.CampusId,
                    payment.PaymentDate);

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
                    payment.GrossPayment,
                    payment.CampusId,
                    payment.PaymentDate
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
                    payment.Discount,
                    payment.CampusId,
                    payment.PaymentDate
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
                    payment.SalesTax,
                    payment.CampusId,
                    payment.PaymentDate
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
                    payment.IncomeTax,
                    payment.CampusId,
                    payment.PaymentDate);
                    await _unitOfWork.Ledger.Add(addIncomeTaxInRecordLedger);
                }
                var addNetPaymentInRecordLedger = new RecordLedger(
                    transaction.Id,
                    payment.PaymentRegisterId,
                    payment.BusinessPartnerId,
                    null,
                    payment.Description,
                    'C',
                    (payment.GrossPayment - payment.Discount - payment.SalesTax - payment.IncomeTax),
                    payment.CampusId,
                    payment.PaymentDate);

                await _unitOfWork.Ledger.Add(addNetPaymentInRecordLedger);
            }

            await _unitOfWork.SaveAsync();
        }

        public async Task<Response<bool>> CheckWorkFlow(ApprovalDto data)
        {
            var getPayment = await _unitOfWork.Payment.GetById(data.DocId, new PaymentSpecs());

            if (getPayment == null)
            {
                return new Response<bool>("Payment with the input id not found");
            }
            if (getPayment.Status.State == DocumentStatus.Unpaid || getPayment.Status.State == DocumentStatus.Partial || getPayment.Status.State == DocumentStatus.Paid)
            {
                return new Response<bool>("Payment already approved");
            }
            var workflow = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.Payment)).FirstOrDefault();

            if (workflow == null)
            {
                return new Response<bool>("No activated workflow found for this document");
            }
            var transition = workflow.WorkflowTransitions
                    .FirstOrDefault(x => (x.CurrentStatusId == getPayment.StatusId && x.Action == data.Action));

            if (transition == null)
            {
                return new Response<bool>("No transition found");
            }
            var currentUserRoles = new GetUser(this._httpContextAccessor).GetCurrentUserRoles();
            _unitOfWork.CreateTransaction();
            try
            {
                foreach (var role in currentUserRoles)
                {
                    if (transition.AllowedRole.Name == role)
                    {
                        getPayment.setStatus(transition.NextStatusId);
                        if (transition.NextStatus.State == DocumentStatus.Unpaid)
                        {
                            await AddToLedger(getPayment);
                            _unitOfWork.Commit();
                            return new Response<bool>(true, "Payment Approved");
                        }
                        if (transition.NextStatus.State == DocumentStatus.Rejected)
                        {
                            await _unitOfWork.SaveAsync();
                            _unitOfWork.Commit();
                            return new Response<bool>(true, "Payment Rejected");
                        }
                        await _unitOfWork.SaveAsync();
                        _unitOfWork.Commit();
                        return new Response<bool>(true, "Payment Reviewed");
                    }
                }

                return new Response<bool>("User does not have allowed role");

            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new Response<bool>(ex.Message);
            }
        }
    }
}
