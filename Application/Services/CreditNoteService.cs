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
                return new PaginationResponse<List<CreditNoteDto>>(_mapper.Map<List<CreditNoteDto>>(crns), "List is empty");

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

            var creditNoteDto = _mapper.Map<CreditNoteDto>(crn);

            creditNoteDto.IsAllowedRole = false;
            var workflow = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.CreditNote)).FirstOrDefault();

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
            var businessPartner = await _unitOfWork.BusinessPartner.GetById(entity.CustomerId);
            crn.setReceivableAccount((Guid)businessPartner.AccountReceivableId);

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
            var businessPartner = await _unitOfWork.BusinessPartner.GetById(entity.CustomerId);
            crn.setReceivableAccount((Guid)businessPartner.AccountReceivableId);

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
            var currentUserRoles = new GetUser(this._httpContextAccessor).GetCurrentUserRoles();
            _unitOfWork.CreateTransaction();
            try
            {
                foreach (var role in currentUserRoles)
                {
                    if (transition.AllowedRole.Name == role)
                    {
                        getCreditNote.setStatus(transition.NextStatusId);
                        if (transition.NextStatus.State == DocumentStatus.Unpaid)
                        {
                            await AddToLedger(getCreditNote);
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
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new Response<bool>(ex.Message);
            }
        }
    }
}
