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
    public class DebitNoteService : IDebitNoteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DebitNoteService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
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

        public async Task<PaginationResponse<List<DebitNoteDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var docDate = new List<DateTime?>();
            var states = new List<DocumentStatus?>();
            if (filter.DocDate != null)
            {
                docDate.Add(filter.DocDate);
            }
            if (filter.State != null)
            {
                states.Add(filter.State);
            }
            var specification = new DebitNoteSpecs(docDate, states, filter);
            var dbns = await _unitOfWork.DebitNote.GetAll(specification);

            if (dbns.Count() == 0)
                return new PaginationResponse<List<DebitNoteDto>>(_mapper.Map<List<DebitNoteDto>>(dbns), "List is empty");

            var totalRecords = await _unitOfWork.DebitNote.TotalRecord(specification);

            return new PaginationResponse<List<DebitNoteDto>>(_mapper.Map<List<DebitNoteDto>>(dbns),
                filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<DebitNoteDto>> GetByIdAsync(int id)
        {
            var specification = new DebitNoteSpecs(false);
            var dbn = await _unitOfWork.DebitNote.GetById(id, specification);
            if (dbn == null)
                return new Response<DebitNoteDto>("Not found");

            var debitNoteDto = _mapper.Map<DebitNoteDto>(dbn);
            
            var workflow = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.DebitNote)).FirstOrDefault();

            if ((debitNoteDto.State == DocumentStatus.Unpaid || debitNoteDto.State == DocumentStatus.Partial || debitNoteDto.State == DocumentStatus.Paid) && debitNoteDto.TransactionId != null)
            {
                return new Response<DebitNoteDto>(MapToValue(debitNoteDto), "Returning value");
            }

            debitNoteDto.IsAllowedRole = false;

            if (dbn.DocumentLedgerId != null)
            {
                var ledger = _unitOfWork.Ledger.Find(new LedgerSpecs((int)dbn.DocumentLedgerId, false)).FirstOrDefault();
                if (ledger != null)
                {
                    debitNoteDto.DocumentReconcile = new PaidDocListDto
                    {
                        Id = ledger.Transactions.DocId,
                        DocNo = ledger.Transactions.DocNo,
                        DocType = ledger.Transactions.DocType,
                        Amount = ledger.Amount
                    };
                }
            }

            if (workflow != null)
            {
                var transition = workflow.WorkflowTransitions
                    .FirstOrDefault(x => (x.CurrentStatusId == debitNoteDto.StatusId));

                if (transition != null)
                {
                    var currentUserRoles = new GetUser(this._httpContextAccessor).GetCurrentUserRoles();
                    foreach (var role in currentUserRoles)
                    {
                        if (transition.AllowedRole.Name == role)
                        {
                            debitNoteDto.IsAllowedRole = true;
                        }
                    }
                }
            }
            return new Response<DebitNoteDto>(debitNoteDto, "Returning value");
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
            var checkingActiveWorkFlows = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.DebitNote)).FirstOrDefault();

            if (checkingActiveWorkFlows == null)
            {
                return new Response<DebitNoteDto>("No workflow found for Debit Note");
            }

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

            //setting BusinessPartnerPayable
            var businessPartner = await _unitOfWork.BusinessPartner.GetById(entity.VendorId);
            dbn.setPayableAccountId((Guid)businessPartner.AccountPayableId);

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

            //setting BusinessPartnerPayable
            var businessPartner = await _unitOfWork.BusinessPartner.GetById(entity.VendorId);
            dbn.setPayableAccountId((Guid)businessPartner.AccountPayableId);

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
            var transaction = new Transactions(dbn.Id, dbn.DocNo, DocType.DebitNote);
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
                    amount + tax,
                    dbn.CampusId,
                    dbn.NoteDate
                    );

                await _unitOfWork.Ledger.Add(addSalesAmountInRecordLedger);
                await _unitOfWork.SaveAsync();
            }
            var getVendorAccount = await _unitOfWork.BusinessPartner.GetById(dbn.VendorId);
            var addPayableInLedger = new RecordLedger(
                        transaction.Id,
                        (Guid)getVendorAccount.AccountPayableId,
                        dbn.VendorId,
                        null,
                        dbn.DocNo,
                        'D',
                        dbn.TotalAmount,
                        dbn.CampusId,
                        dbn.NoteDate
                    );

            await _unitOfWork.Ledger.Add(addPayableInLedger);
            await _unitOfWork.SaveAsync();
        }

        public async Task<Response<bool>> CheckWorkFlow(ApprovalDto data)
        {
            var getDebitNote = await _unitOfWork.DebitNote.GetById(data.DocId, new DebitNoteSpecs(true));

            if (getDebitNote == null)
            {
                return new Response<bool>("DebitNote with the input id not found");
            }
            if (getDebitNote.Status.State == DocumentStatus.Unpaid || getDebitNote.Status.State == DocumentStatus.Partial || getDebitNote.Status.State == DocumentStatus.Paid)
            {
                return new Response<bool>("DebitNote already approved");
            }
            var workflow = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.DebitNote)).FirstOrDefault();

            if (workflow == null)
            {
                return new Response<bool>("No activated workflow found for this document");
            }
            var transition = workflow.WorkflowTransitions
                    .FirstOrDefault(x => (x.CurrentStatusId == getDebitNote.StatusId && x.Action == data.Action));

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
                        getDebitNote.setStatus(transition.NextStatusId);
                        if (transition.NextStatus.State == DocumentStatus.Unpaid)
                        {
                            await AddToLedger(getDebitNote);

                            if (getDebitNote.DocumentLedgerId != 0 && getDebitNote.DocumentLedgerId != null)
                            {
                                TransactionReconcileService trecon = new TransactionReconcileService(_unitOfWork);
                                //Getting transaction with Payment Transaction Id
                                var getUnreconciledDocumentAmount = _unitOfWork.Ledger.Find(new LedgerSpecs(true, (int)getDebitNote.TransactionId)).FirstOrDefault();
                                if (getUnreconciledDocumentAmount == null)
                                {
                                    _unitOfWork.Rollback();
                                    return new Response<bool>("Ledger not found");
                                }

                                var reconModel = new CreateTransactionReconcileDto()
                                {
                                    PaymentLedgerId = getUnreconciledDocumentAmount.Id,
                                    DocumentLedgerId = (int)getDebitNote.DocumentLedgerId,
                                    Amount = getDebitNote.TotalAmount
                                };

                                //Checking Reconciliation Validation
                                var checkValidation = trecon.CheckReconValidation(reconModel);
                                if (!checkValidation.IsSuccess)
                                {
                                    _unitOfWork.Rollback();
                                    return new Response<bool>(checkValidation.Message);
                                }

                                var reconcile = await trecon.ReconciliationProcess(reconModel);
                                if (!reconcile.IsSuccess)
                                {
                                    _unitOfWork.Rollback();
                                    return new Response<bool>(reconcile.Message);
                                }
                            }

                            _unitOfWork.Commit();
                            return new Response<bool>(true, "DebitNote Approved");
                        }
                        if (transition.NextStatus.State == DocumentStatus.Rejected)
                        {
                            await _unitOfWork.SaveAsync();
                            _unitOfWork.Commit();
                            return new Response<bool>(true, "DebitNote Rejected");
                        }
                        await _unitOfWork.SaveAsync();
                        _unitOfWork.Commit();
                        return new Response<bool>(true, "DebitNote Reviewed");
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

        private DebitNoteDto MapToValue(DebitNoteDto data)
        {
            //Getting transaction with Payment Transaction Id
            var getUnreconciledDocumentAmount = _unitOfWork.Ledger.Find(new LedgerSpecs((int)data.TransactionId, true)).FirstOrDefault();

            // Checking if given amount is greater than unreconciled document amount
            var transactionReconciles = _unitOfWork.TransactionReconcile.Find(new TransactionReconSpecs(getUnreconciledDocumentAmount.Id, true)).ToList();

            //For Paid Document List
            var paidDocList = new List<PaidDocListDto>();
            // if reconciles transaction found

            if (transactionReconciles.Count() > 0)
            {
                //Adding Paid Doc List
                foreach (var tranRecon in transactionReconciles)
                {
                    paidDocList.Add(new PaidDocListDto
                    {
                        Id = tranRecon.DocumentLedger.Transactions.DocId,
                        DocNo = tranRecon.DocumentLedger.Transactions.DocNo,
                        DocType = tranRecon.DocumentLedger.Transactions.DocType,
                        Amount = tranRecon.Amount
                    });
                }
            }

            //Getting Pending DebitNoteDto Amount
            var unReconciledAmount = data.TotalAmount - transactionReconciles.Sum(e => e.Amount);

            data.LedgerId = getUnreconciledDocumentAmount.Id;
            data.ReconciledAmount = transactionReconciles.Sum(e => e.Amount);
            data.PaidAmountList = paidDocList;
            data.UnreconciledAmount = unReconciledAmount;

            // Returning DebitNoteDto with all values assigned
            return data;
        }
    }
}
