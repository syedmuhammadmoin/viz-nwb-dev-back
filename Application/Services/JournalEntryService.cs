using Application.Contracts.DTOs;
using Application.Contracts.DTOs.FileUpload;
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
    public class JournalEntryService : IJournalEntryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public JournalEntryService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Response<JournalEntryDto>> CreateAsync(CreateJournalEntryDto entity)
        {
            if (entity.isSubmit)
            {
                return await this.SubmitJV(entity);
            }
            else
            {
                return await this.SaveJV(entity, 1);
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
                return await this.UpdateJV(entity, 1);
            }
        }

        public async Task<PaginationResponse<List<JournalEntryDto>>> GetAllAsync(TransactionFormFilter filter)
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
            var jvs = await _unitOfWork.JournalEntry.GetAll(new JournalEntrySpecs(docDate, dueDate, states, filter, false));

            if (jvs.Count() == 0)
                return new PaginationResponse<List<JournalEntryDto>>(_mapper.Map<List<JournalEntryDto>>(jvs), "List is empty");

            var totalRecords = await _unitOfWork.JournalEntry.TotalRecord(new JournalEntrySpecs(docDate, dueDate, states, filter, true));

            return new PaginationResponse<List<JournalEntryDto>>(_mapper.Map<List<JournalEntryDto>>(jvs),
                filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<JournalEntryDto>> GetByIdAsync(int id)
        {
            var specification = new JournalEntrySpecs(false);
            var jv = await _unitOfWork.JournalEntry.GetById(id, specification);
            if (jv == null)
                return new Response<JournalEntryDto>("Not found");


            var jVDto = _mapper.Map<JournalEntryDto>(jv);
            ReturningRemarks(jVDto, DocType.JournalEntry);



            ReturningFiles(jVDto, DocType.JournalEntry);
       
            jVDto.IsAllowedRole = false;
            var workflow = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.JournalEntry)).FirstOrDefault();


            if (workflow != null)
            {
                var transition = workflow.WorkflowTransitions
                    .FirstOrDefault(x => (x.CurrentStatusId == jVDto.StatusId));

                if (transition != null)
                {
                    var currentUserRoles = new GetUser(this._httpContextAccessor).GetCurrentUserRoles();
                    foreach (var role in currentUserRoles)
                    {
                        if (transition.AllowedRole.Name == role)
                        {
                            jVDto.IsAllowedRole = true;
                        }
                    }
                }
            }
            return new Response<JournalEntryDto>(jVDto, "Returning value");
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        //Private Methods for JournalEntry
        private async Task<Response<JournalEntryDto>> SubmitJV(CreateJournalEntryDto entity)
        {
            var checkingActiveWorkFlows = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.JournalEntry)).FirstOrDefault();

            if (checkingActiveWorkFlows == null)
            {
                return new Response<JournalEntryDto>("No workflow found for journal voucher");
            }
            if (entity.Id == null)
            {
                return await this.SaveJV(entity, 6);
            }
            else
            {
                return await this.UpdateJV(entity, 6);
            }
        }

        private async Task<Response<JournalEntryDto>> SaveJV(CreateJournalEntryDto entity, int status)
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
                JournalEntryDto data = new JournalEntryDto();
                var remarks = _unitOfWork.Remarks.Find(new RemarksSpecs(data.Id, DocType.JournalEntry))
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
                //For creating docNo
                jv.CreateDocNo();
                await _unitOfWork.SaveAsync();
                //Remarks


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

        private async Task<Response<JournalEntryDto>> UpdateJV(CreateJournalEntryDto entity, int status)
        {
            if (entity.JournalEntryLines.Count() == 0)
                return new Response<JournalEntryDto>("Lines are required");

            var totalDebit = entity.JournalEntryLines.Sum(i => i.Debit);
            var totalCredit = entity.JournalEntryLines.Sum(i => i.Credit);

            if (totalDebit != totalCredit)
                return new Response<JournalEntryDto>("Sum of debit and credit must be equal");

            var specification = new JournalEntrySpecs(true);
            var jv = await _unitOfWork.JournalEntry.GetById((int)entity.Id, specification);

            if (jv == null)
                return new Response<JournalEntryDto>("Not found");

            if (jv.StatusId != 1 && jv.StatusId != 2)
                return new Response<JournalEntryDto>("Journal voucher already submitted");

            jv.setStatus(status);

            _unitOfWork.CreateTransaction();
            try
            {
                //For updating data
                _mapper.Map<CreateJournalEntryDto, JournalEntryMaster>(entity, jv);

                await _unitOfWork.SaveAsync();

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
            var transaction = new Transactions(jv.Id, jv.DocNo, DocType.JournalEntry);
            var addTransaction = await _unitOfWork.Transaction.Add(transaction);
            await _unitOfWork.SaveAsync();

            jv.setTransactionId(transaction.Id);
            await _unitOfWork.SaveAsync();

            //Inserting data into recordledger table
            List<RecordLedger> recordLedger = jv.JournalEntryLines
                .Select(line => new RecordLedger(
                    transaction.Id,
                    line.AccountId,
                    line.BusinessPartnerId,
                    line.WarehouseId,
                    line.Description,
                    line.Debit > 0 && line.Credit <= 0 ? 'D' : 'C',
                    line.Debit > 0 && line.Credit <= 0 ? line.Debit : line.Credit,
                    jv.CampusId,
                    jv.Date
                    )).ToList();

            await _unitOfWork.Ledger.AddRange(recordLedger);
            await _unitOfWork.SaveAsync();
        }

        public async Task<Response<bool>> CheckWorkFlow(ApprovalDto data)
        {
            var getJournalEntry = await _unitOfWork.JournalEntry.GetById(data.DocId, new JournalEntrySpecs(true));

            if (getJournalEntry == null)
            {
                return new Response<bool>("JournalEntry with the input id not found");
            }
            if (getJournalEntry.Status.State == DocumentStatus.Unpaid || getJournalEntry.Status.State == DocumentStatus.Partial || getJournalEntry.Status.State == DocumentStatus.Paid)
            {
                return new Response<bool>("JournalEntry already approved");
            }
            var workflow = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.JournalEntry)).FirstOrDefault();

            if (workflow == null)
            {
                return new Response<bool>("No activated workflow found for this document");
            }
            var transition = workflow.WorkflowTransitions
                    .FirstOrDefault(x => (x.CurrentStatusId == getJournalEntry.StatusId && x.Action == data.Action));

            if (transition == null)
            {
                return new Response<bool>("No transition found");
            }
            // Creating object of getUSer class
            var getUser = new GetUser(this._httpContextAccessor);

            var userId = getUser.GetCurrentUserId();
            var currentUserRoles = new GetUser(this._httpContextAccessor).GetCurrentUserRoles();
       
            _unitOfWork.CreateTransaction();
            try
            {
                foreach (var role in currentUserRoles)
                {
                    if (transition.AllowedRole.Name == role)
                    {
                        getJournalEntry.setStatus(transition.NextStatusId);
                        if (!String.IsNullOrEmpty(data.Remarks))
                        {
                            var addRemarks = new Remark()
                            {
                                DocId = getJournalEntry.Id,
                                DocType = DocType.JournalEntry,
                                Remarks = data.Remarks,
                                UserId = userId
                            };
                            await _unitOfWork.Remarks.Add(addRemarks);
                        }
              
                        if (transition.NextStatus.State == DocumentStatus.Unpaid)
                        {
                            await AddToLedger(getJournalEntry);
                            _unitOfWork.Commit();
                            return new Response<bool>(true, "JournalEntry Approved");
                        }
                 
                        if (transition.NextStatus.State == DocumentStatus.Rejected)
                        {
                            await _unitOfWork.SaveAsync();
                            _unitOfWork.Commit();
                            return new Response<bool>(true, "JournalEntry Rejected");
                        }
                        await _unitOfWork.SaveAsync();
                        _unitOfWork.Commit();
                        return new Response<bool>(true, "JournalEntry Reviewed");
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

        private List<RemarksDto> ReturningRemarks(JournalEntryDto data, DocType docType)
        {
            var remarks = _unitOfWork.Remarks.Find(new RemarksSpecs(data.Id, DocType.JournalEntry))
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
  
        private List<FileUploadDto> ReturningFiles(JournalEntryDto data, DocType docType)
        {

            var files = _unitOfWork.Fileupload.Find(new FileUploadSpecs(data.Id, DocType.JournalEntry))
                    .Select(e => new FileUploadDto()
                    {
                        Id = e.Id,
                        Name = e.Name,
                        DocType = DocType.JournalEntry,
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
