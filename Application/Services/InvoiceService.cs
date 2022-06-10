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
    public class InvoiceService : IInvoiceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public InvoiceService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Response<InvoiceDto>> CreateAsync(CreateInvoiceDto entity)
        {
            if (entity.isSubmit)
            {
                return await this.SubmitINV(entity);
            }
            else
            {
                return await this.SaveINV(entity, 1);
            }
        }

        public async Task<PaginationResponse<List<InvoiceDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var docDate = new List<DateTime?>();
            var dueDate = new List<DateTime?>();
            var states = new List<DocumentStatus?>();
            if (filter.DocDate != null)
            {
                docDate.Add(filter.DocDate);
            }
            if (filter.DueDate != null)
            {
                dueDate.Add(filter.DueDate);
            }
            if (filter.State != null)
            {
                states.Add(filter.State);
            }
            var specification = new InvoiceSpecs(docDate, dueDate, states, filter);
            var Invs = await _unitOfWork.Invoice.GetAll(specification);

            if (Invs.Count() == 0)
                return new PaginationResponse<List<InvoiceDto>>(_mapper.Map<List<InvoiceDto>>(Invs), "List is empty");

            var totalRecords = await _unitOfWork.Invoice.TotalRecord(specification);

            return new PaginationResponse<List<InvoiceDto>>(_mapper.Map<List<InvoiceDto>>(Invs),
                filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<InvoiceDto>> GetByIdAsync(int id)
        {
            var specification = new InvoiceSpecs(false);
            var inv = await _unitOfWork.Invoice.GetById(id, specification);
            if (inv == null)
                return new Response<InvoiceDto>("Not found");

            var invoiceDto = _mapper.Map<InvoiceDto>(inv);

            if ((invoiceDto.State == DocumentStatus.Unpaid || invoiceDto.State == DocumentStatus.Partial || invoiceDto.State == DocumentStatus.Paid) && invoiceDto.TransactionId != null)
            {
                return new Response<InvoiceDto>(MapToValue(invoiceDto), "Returning value");
            }

            invoiceDto.IsAllowedRole = false;
            var workflow = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.Invoice)).FirstOrDefault();
            if (workflow != null)
            {
                var transition = workflow.WorkflowTransitions
                    .FirstOrDefault(x => (x.CurrentStatusId == invoiceDto.StatusId));

                if (transition != null)
                {
                    var currentUserRoles = new GetUser(this._httpContextAccessor).GetCurrentUserRoles();
                    foreach (var role in currentUserRoles)
                    {
                        if (transition.AllowedRole.Name == role)
                        {
                            invoiceDto.IsAllowedRole = true;
                        }
                    }
                }
            }
            return new Response<InvoiceDto>(invoiceDto, "Returning value");
        }

        public async Task<Response<InvoiceDto>> UpdateAsync(CreateInvoiceDto entity)
        {
            if (entity.isSubmit)
            {
                return await this.SubmitINV(entity);
            }
            else
            {
                return await this.UpdateINV(entity, 1);
            }
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<bool>> CheckWorkFlow(ApprovalDto data)
        {
            var getInvoice = await _unitOfWork.Invoice.GetById(data.DocId, new InvoiceSpecs(true));

            if (getInvoice == null)
            {
                return new Response<bool>("Invoice with the input id not found");
            }
            if (getInvoice.Status.State == DocumentStatus.Unpaid || getInvoice.Status.State == DocumentStatus.Partial || getInvoice.Status.State == DocumentStatus.Paid)
            {
                return new Response<bool>("Invoice already approved");
            }
            var workflow = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.Invoice)).FirstOrDefault();

            if (workflow == null)
            {
                return new Response<bool>("No activated workflow found for this document");
            }
            var transition = workflow.WorkflowTransitions
                    .FirstOrDefault(x => (x.CurrentStatusId == getInvoice.StatusId && x.Action == data.Action));

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
                        getInvoice.setStatus(transition.NextStatusId);
                        if (transition.NextStatus.State == DocumentStatus.Unpaid)
                        {
                            await AddToLedger(getInvoice);
                            _unitOfWork.Commit();
                            return new Response<bool>(true, "Invoice Approved");
                        }
                        if (transition.NextStatus.State == DocumentStatus.Rejected)
                        {
                            await _unitOfWork.SaveAsync();
                            _unitOfWork.Commit();
                            return new Response<bool>(true, "Invoice Rejected");
                        }
                        await _unitOfWork.SaveAsync();
                        _unitOfWork.Commit();
                        return new Response<bool>(true, "Invoice Reviewed");
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

        //Private Methods for Invoice
        private async Task<Response<InvoiceDto>> SubmitINV(CreateInvoiceDto entity)
        {
            var checkingActiveWorkFlows = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.Invoice)).FirstOrDefault();

            if (checkingActiveWorkFlows == null)
            {
                return new Response<InvoiceDto>("No workflow found for Invoice");
            }
            if (entity.Id == null)
            {
                return await this.SaveINV(entity, 6);
            }
            else
            {
                return await this.UpdateINV(entity, 6);
            }
        }

        private async Task<Response<InvoiceDto>> SaveINV(CreateInvoiceDto entity, int status)
        {
            if (entity.InvoiceLines.Count() == 0)
                return new Response<InvoiceDto>("Lines are required");

            var inv = _mapper.Map<InvoiceMaster>(entity);

            //setting BusinessPartnerReceivable
            var businessPartner = await _unitOfWork.BusinessPartner.GetById(entity.CustomerId);
            inv.setReceivableAccount((Guid)businessPartner.AccountReceivableId);

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

        private async Task<Response<InvoiceDto>> UpdateINV(CreateInvoiceDto entity, int status)
        {
            if (entity.InvoiceLines.Count() == 0)
                return new Response<InvoiceDto>("Lines are required");

            var specification = new InvoiceSpecs(true);
            var inv = await _unitOfWork.Invoice.GetById((int)entity.Id, specification);

            if (inv == null)
                return new Response<InvoiceDto>("Not found");

            if (inv.StatusId != 1 && inv.StatusId != 2)
                return new Response<InvoiceDto>("Only draft document can be edited");



            inv.setStatus(status);

            _unitOfWork.CreateTransaction();
            try
            {
                //For updating data
                _mapper.Map<CreateInvoiceDto, InvoiceMaster>(entity, inv);

                //setting BusinessPartnerReceivable
                var businessPartner = await _unitOfWork.BusinessPartner.GetById(entity.CustomerId);
                inv.setReceivableAccount((Guid)businessPartner.AccountReceivableId);
                await _unitOfWork.SaveAsync();

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
                    line.WarehouseId,
                    line.Description,
                    'C',
                    line.Price * line.Quantity,
                    inv.CampusId,
                    inv.InvoiceDate
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
                        line.WarehouseId,
                        line.Description,
                        'C',
                        tax,
                        inv.CampusId,
                        inv.InvoiceDate
                    );
                    await _unitOfWork.Ledger.Add(addSalesTaxInRecordLedger);
                    await _unitOfWork.SaveAsync();
                }
            }
            var getCustomerAccount = await _unitOfWork.BusinessPartner.GetById(inv.CustomerId);
            var addReceivableInLedger = new RecordLedger(
                        transaction.Id,
                        (Guid)getCustomerAccount.AccountReceivableId,
                        inv.CustomerId,
                        null,
                        inv.DocNo,
                        'D',
                        inv.TotalAmount,
                        inv.CampusId,
                        inv.InvoiceDate
                    );

            await _unitOfWork.Ledger.Add(addReceivableInLedger);
            await _unitOfWork.SaveAsync();
        }

        private InvoiceDto MapToValue(InvoiceDto data)
        {
            //Getting transaction with Payment Transaction Id
            var getUnreconciledDocumentAmount = _unitOfWork.Ledger.Find(new LedgerSpecs((int)data.TransactionId, true)).FirstOrDefault();
            
            // Checking if given amount is greater than unreconciled document amount
            var transactionReconciles = _unitOfWork.TransactionReconcile.Find(new TransactionReconSpecs(getUnreconciledDocumentAmount.Id, false)).ToList();

            //For Paid Document List
            var paidDocList = new List<PaidDocListDto>();
            // if reconciles transaction found
            if (transactionReconciles.Count() > 0)
            {
                //Adding Paid Doc List
                foreach (var tranRecon in transactionReconciles)
                {
                    string[] docId = tranRecon.PaymentLedger.Transactions.DocNo.Split("-");
                        paidDocList.Add(new PaidDocListDto
                    {
                        Id = Int32.Parse(docId[1]),
                        DocNo = tranRecon.PaymentLedger.Transactions.DocNo,
                        DocType = tranRecon.PaymentLedger.Transactions.DocType,
                        Amount = tranRecon.Amount
                    });
                }
            }

            //Getting Pending Invoice Amount
            var pendingAmount = data.TotalAmount - transactionReconciles.Sum(e => e.Amount);

            if (data.State != DocumentStatus.Paid)
            {
                //Creating transactionReconRepo object
                TransactionReconcileService trasactionReconService = new TransactionReconcileService(_unitOfWork);
                var getUnreconPayment = trasactionReconService.GetPaymentReconAmounts(getUnreconciledDocumentAmount.Level4_id, (int)getUnreconciledDocumentAmount.BusinessPartnerId, getUnreconciledDocumentAmount.Sign);


                //For Getting Business Partner Unreconciled Payments and CreditNote
                var BPUnreconPayments = getUnreconPayment.Result.Select(i => new UnreconciledBusinessPartnerPaymentsDto()
                {
                    Id = i.DocumentId,
                    DocNo = i.DocNo,
                    DocType = i.DocType,
                    Amount = i.UnreconciledAmount,
                    PaymentLedgerId = i.PaymentLedgerId
                }).ToList();
                
                data.BPUnreconPaymentList = BPUnreconPayments;
            }

            data.TotalPaid = transactionReconciles.Sum(e => e.Amount);
            data.PaidAmountList = paidDocList;
            data.PendingAmount = pendingAmount;
            data.LedgerId = getUnreconciledDocumentAmount.Id;

            // Returning invoiceDTO with all values assigned
            return data;
        }

    }
}
