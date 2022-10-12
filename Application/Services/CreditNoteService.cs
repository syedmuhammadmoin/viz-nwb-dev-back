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
    public class CreditNoteService : ICreditNoteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreditNoteService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Response<CreditNoteDto>> CreateAsync(CreateCreditNoteDto entity)
        {
            if ((bool)entity.isSubmit)
            {
                return await this.SubmitCRN(entity);
            }
            else
            {
                return await this.SaveCRN(entity, 1);
            }
        }

        public async Task<PaginationResponse<List<CreditNoteDto>>> GetAllAsync(TransactionFormFilter filter)
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

            var crns = await _unitOfWork.CreditNote.GetAll(new CreditNoteSpecs(docDate, states, filter, false));

            if (crns.Count() == 0)
                return new PaginationResponse<List<CreditNoteDto>>(_mapper.Map<List<CreditNoteDto>>(crns), "List is empty");

            var totalRecords = await _unitOfWork.CreditNote.TotalRecord(new CreditNoteSpecs(docDate, states, filter, true));

            return new PaginationResponse<List<CreditNoteDto>>(_mapper.Map<List<CreditNoteDto>>(crns),
                filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<CreditNoteDto>> GetByIdAsync(int id)
        {
            var specification = new CreditNoteSpecs(false);
            var crn = await _unitOfWork.CreditNote.GetById(id, specification);
            if (crn == null)
                return new Response<CreditNoteDto>("Not found");

            var creditNoteDto = _mapper.Map<CreditNoteDto>(crn);
            //Returning
            ReturningRemarks(creditNoteDto, DocType.CreditNote); 

            //Returning
      
            ReturningFiles(creditNoteDto, DocType.CreditNote); 

            var workflow = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.CreditNote)).FirstOrDefault();
            if ((creditNoteDto.State == DocumentStatus.Unpaid || creditNoteDto.State == DocumentStatus.Partial || creditNoteDto.State == DocumentStatus.Paid) && creditNoteDto.TransactionId != null)
            {
                return new Response<CreditNoteDto>(MapToValue(creditNoteDto), "Returning value");
            }

            creditNoteDto.IsAllowedRole = false;

            if (crn.DocumentLedgerId != null)
            {
                var ledger = _unitOfWork.Ledger.Find(new LedgerSpecs((int)crn.DocumentLedgerId, false)).FirstOrDefault();
                if (ledger != null)
                {
                    creditNoteDto.DocumentReconcile = new PaidDocListDto
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
                    .FirstOrDefault(x => (x.CurrentStatusId == creditNoteDto.StatusId));

                if (transition != null)
                {
                    var currentUserRoles = new GetUser(this._httpContextAccessor).GetCurrentUserRoles();
                    foreach (var role in currentUserRoles)
                    {
                        if (transition.AllowedRole.Name == role)
                        {
                            creditNoteDto.IsAllowedRole = true;
                        }
                    }
                }
            }
            return new Response<CreditNoteDto>(creditNoteDto, "Returning value");
        }

        public async Task<Response<CreditNoteDto>> UpdateAsync(CreateCreditNoteDto entity)
        {
            if ((bool)entity.isSubmit)
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
            var checkingActiveWorkFlows = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.CreditNote)).FirstOrDefault();

            if (checkingActiveWorkFlows == null)
            {
                return new Response<CreditNoteDto>("No workflow found for Credit Note");
            }

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
            var businessPartner = await _unitOfWork.BusinessPartner.GetById((int)entity.CustomerId);
            crn.setReceivableAccount((Guid)businessPartner.AccountReceivableId);

            //Setting status
            crn.setStatus(status);

            _unitOfWork.CreateTransaction();
          
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
            var businessPartner = await _unitOfWork.BusinessPartner.GetById((int)entity.CustomerId);
            crn.setReceivableAccount((Guid)businessPartner.AccountReceivableId);

            crn.setStatus(status);

            _unitOfWork.CreateTransaction();
            
                //For updating data
                _mapper.Map<CreateCreditNoteDto, CreditNoteMaster>(entity, crn);

                await _unitOfWork.SaveAsync();

                //Commiting the transaction
                _unitOfWork.Commit();

                //returning response
                return new Response<CreditNoteDto>(_mapper.Map<CreditNoteDto>(crn), "Created successfully");
            
         
        }

        private async Task AddToLedger(CreditNoteMaster crn)
        {
            var transaction = new Transactions(crn.Id, crn.DocNo, DocType.CreditNote);
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
                    line.Price * line.Quantity,
                    crn.CampusId,
                    crn.NoteDate
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
                        tax,
                        crn.CampusId,
                        crn.NoteDate
                        );
                    await _unitOfWork.Ledger.Add(addSalesTaxInRecordLedger);
                    await _unitOfWork.SaveAsync();
                }
            }
            var getCustomerAccount = await _unitOfWork.BusinessPartner.GetById(crn.CustomerId);
            var addReceivableInLedger = new RecordLedger(
                        transaction.Id,
                        (Guid)getCustomerAccount.AccountReceivableId,
                        crn.CustomerId,
                        null,
                        crn.DocNo,
                        'C',
                        crn.TotalAmount,
                        crn.CampusId,
                        crn.NoteDate
                    );

            await _unitOfWork.Ledger.Add(addReceivableInLedger);
            await _unitOfWork.SaveAsync();

            //Getting transaction with Payment Transaction Id
            var getUnreconciledDocumentAmount = _unitOfWork.Ledger.Find(new LedgerSpecs(transaction.Id, true)).FirstOrDefault();
            
            crn.setLedgerId(getUnreconciledDocumentAmount.Id);
            await _unitOfWork.SaveAsync();
        }

        public async Task<Response<bool>> CheckWorkFlow(ApprovalDto data)
        {
            var getCreditNote = await _unitOfWork.CreditNote.GetById(data.DocId, new CreditNoteSpecs(true));

            if (getCreditNote == null)
            {
                return new Response<bool>("CreditNote with the input id not found");
            }
            if (getCreditNote.Status.State == DocumentStatus.Unpaid || getCreditNote.Status.State == DocumentStatus.Partial || getCreditNote.Status.State == DocumentStatus.Paid)
            {
                return new Response<bool>("CreditNote already approved");
            }
            var workflow = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.CreditNote)).FirstOrDefault();

            if (workflow == null)
            {
                return new Response<bool>("No activated workflow found for this document");
            }
            var transition = workflow.WorkflowTransitions
                    .FirstOrDefault(x => (x.CurrentStatusId == getCreditNote.StatusId && x.Action == data.Action));

            if (transition == null)
            {
                return new Response<bool>("No transition found");
            }

            // Creating object of getUSer class
            var getUser = new GetUser(this._httpContextAccessor);

            var userId = getUser.GetCurrentUserId();

            var currentUserRoles = new GetUser(this._httpContextAccessor).GetCurrentUserRoles();
            _unitOfWork.CreateTransaction();
            
                foreach (var role in currentUserRoles)
                {
                    if (transition.AllowedRole.Name == role)
                    {
                        getCreditNote.setStatus(transition.NextStatusId);

                        if (!String.IsNullOrEmpty(data.Remarks))
                        {
                            var addRemarks = new Remark()
                            {
                                DocId = getCreditNote.Id,
                                DocType = DocType.CreditNote,
                                Remarks = data.Remarks,
                                UserId = userId
                            };
                            await _unitOfWork.Remarks.Add(addRemarks);
                        }

                        if (transition.NextStatus.State == DocumentStatus.Unpaid)
                        {
                            await AddToLedger(getCreditNote);

                            if (getCreditNote.DocumentLedgerId != 0 && getCreditNote.DocumentLedgerId != null)
                            {
                                TransactionReconcileService trecon = new TransactionReconcileService(_unitOfWork);
                                //Getting transaction with Payment Transaction Id
                                var getUnreconciledDocumentAmount = _unitOfWork.Ledger.Find(new LedgerSpecs(true, (int)getCreditNote.TransactionId)).FirstOrDefault();
                                if (getUnreconciledDocumentAmount == null)
                                {
                                    _unitOfWork.Rollback();
                                    return new Response<bool>("Ledger not found");
                                }

                                var reconModel = new CreateTransactionReconcileDto()
                                {
                                    PaymentLedgerId = getUnreconciledDocumentAmount.Id,
                                    DocumentLedgerId = (int)getCreditNote.DocumentLedgerId,
                                    Amount = getCreditNote.TotalAmount
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
                            return new Response<bool>(true, "CreditNote Approved");
                        }
                        if (transition.NextStatus.State == DocumentStatus.Rejected)
                        {
                            await _unitOfWork.SaveAsync();
                            _unitOfWork.Commit();
                            return new Response<bool>(true, "CreditNote Rejected");
                        }
                        await _unitOfWork.SaveAsync();
                        _unitOfWork.Commit();
                        return new Response<bool>(true, "CreditNote Reviewed");
                    }
                }

                return new Response<bool>("User does not have allowed role");

          
        }

        private CreditNoteDto MapToValue(CreditNoteDto data)
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
                    string[] docId = tranRecon.DocumentLedger.Transactions.DocNo.Split("-");
                    paidDocList.Add(new PaidDocListDto
                    {
                        Id = tranRecon.DocumentLedger.Transactions.DocId,
                        DocNo = tranRecon.DocumentLedger.Transactions.DocNo,
                        DocType = tranRecon.DocumentLedger.Transactions.DocType,
                        Amount = tranRecon.Amount
                    });
                }
            }

            //Getting Pending CreditNoteDto Amount
            var unReconciledAmount = data.TotalAmount - transactionReconciles.Sum(e => e.Amount);

            data.ReconciledAmount = transactionReconciles.Sum(e => e.Amount);
            data.PaidAmountList = paidDocList;
            data.UnreconciledAmount = unReconciledAmount;

            // Returning CreditNoteDto with all values assigned
            return data;
        }
        private List<RemarksDto> ReturningRemarks(CreditNoteDto data, DocType docType)
        {
            var remarks = _unitOfWork.Remarks.Find(new RemarksSpecs(data.Id, DocType.CreditNote))
                    .Select(e => new RemarksDto()
                    {
                        Remarks = e.Remarks,
                        UserName = e.User.UserName,
                        CreatedAt = e.CreatedDate == null ? "N/A" : ((DateTime)e.CreatedDate).ToString("ddd, dd MMM yyyy")
                    }).ToList();

            if (remarks.Count() > 0)
            {
                data.RemarksList = _mapper.Map<List<RemarksDto>>(remarks);
            }

            return remarks;
        }
       
        private List<FileUploadDto> ReturningFiles(CreditNoteDto data, DocType docType)
        {

            var files = _unitOfWork.Fileupload.Find(new FileUploadSpecs(data.Id, DocType.CreditNote))
                    .Select(e => new FileUploadDto()
                    {
                        Id = e.Id,
                        Name = e.Name,
                        DocType = DocType.CreditNote,
                        Extension = e.Extension,
                        UserName = e.User.UserName,
                        CreatedAt = e.CreatedDate == null ? "N/A" : ((DateTime)e.CreatedDate).ToString("ddd, dd MMM yyyy")
                    }).ToList();

            if (files.Count() > 0)
            {
                data.FileUploadList = _mapper.Map<List<FileUploadDto>>(files);

            }

            return files;

        }
    }
}
