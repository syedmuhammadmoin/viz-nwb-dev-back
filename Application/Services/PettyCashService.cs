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
    public class PettyCashService : IPettyCashService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public PettyCashService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Response<PettyCashDto>> CreateAsync(CreatePettyCashDto entity)
        {
            if ((bool)entity.isSubmit)
            {
                return await this.Submit(entity);
            }
            else
            {
                return await this.Save(entity, 1);
            }
        }

        public async Task<Response<PettyCashDto>> UpdateAsync(CreatePettyCashDto entity)
        {
            if ((bool)entity.isSubmit)
            {
                return await this.Submit(entity);
            }
            else
            {
                return await this.Update(entity, 1);
            }
        }

        public async Task<PaginationResponse<List<PettyCashDto>>> GetAllAsync(TransactionFormFilter filter)
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
            var pettyCashList = await _unitOfWork.PettyCash.GetAll(new PettyCashSpecs(docDate, dueDate, states, filter, false));

            if (pettyCashList.Count() == 0)
                return new PaginationResponse<List<PettyCashDto>>(_mapper.Map<List<PettyCashDto>>(pettyCashList), "List is empty");

            var totalRecords = await _unitOfWork.PettyCash.TotalRecord(new PettyCashSpecs(docDate, dueDate, states, filter, true));

            return new PaginationResponse<List<PettyCashDto>>(_mapper.Map<List<PettyCashDto>>(pettyCashList),
                filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<PettyCashDto>> GetByIdAsync(int id)
        {
            var specification = new PettyCashSpecs(false);
            var pettyCash = await _unitOfWork.PettyCash.GetById(id, specification);
            if (pettyCash == null)
                return new Response<PettyCashDto>("Not found");


            var pettyCashDto = _mapper.Map<PettyCashDto>(pettyCash);
            ReturningRemarks(pettyCashDto, DocType.PettyCash);



            ReturningFiles(pettyCashDto, DocType.PettyCash);

            pettyCashDto.IsAllowedRole = false;
            var workflow = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.PettyCash)).FirstOrDefault();


            if (workflow != null)
            {
                var transition = workflow.WorkflowTransitions
                    .FirstOrDefault(x => (x.CurrentStatusId == pettyCashDto.StatusId));

                if (transition != null)
                {
                    var currentUserRoles = new GetUser(this._httpContextAccessor).GetCurrentUserRoles();
                    foreach (var role in currentUserRoles)
                    {
                        if (transition.AllowedRole.Name == role)
                        {
                            pettyCashDto.IsAllowedRole = true;
                        }
                    }
                }
            }
            return new Response<PettyCashDto>(pettyCashDto, "Returning value");
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        //Private Methods for PettyCash
        private async Task<Response<PettyCashDto>> Submit(CreatePettyCashDto entity)
        {


            if (!await _unitOfWork.WorkFlow.Any(new WorkFlowSpecs(DocType.PettyCash)))
            {
                return new Response<PettyCashDto>("No workflow found for Petty Cash");
            }
            if (entity.Id == null)
            {
                return await this.Save(entity, 6);
            }
            else
            {
                return await this.Update(entity, 6);
            }
        }

        private async Task<Response<PettyCashDto>> Save(CreatePettyCashDto entity, int status)
        {
            if (entity.PettyCashLines.Count() == 0)
                return new Response<PettyCashDto>("Lines are required");

            var pettyCash = _mapper.Map<PettyCashMaster>(entity);

            foreach (var lines in entity.PettyCashLines)
            {
                if(lines.AccountId == entity.AccountId)
                {
                    return new Response<PettyCashDto>("Cannot select same account in lines");
                }
                if (lines.Debit > 0 && lines.Credit > 0)
                    return new Response<PettyCashDto>("Debit and Credit amount should be in seperate lines");
            }

            


            //Setting status
            pettyCash.SetStatus(status);

            _unitOfWork.CreateTransaction();



            //Saving in table
            var result = await _unitOfWork.PettyCash.Add(pettyCash);
            await _unitOfWork.SaveAsync();
            PettyCashDto data = new PettyCashDto();
            var remarks = _unitOfWork.Remarks.Find(new RemarksSpecs(data.Id, DocType.PettyCash))
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
            pettyCash.CreateDocNo();
            await _unitOfWork.SaveAsync();
            //Remarks


            //Commiting the transaction
            _unitOfWork.Commit();

            //returning response
            return new Response<PettyCashDto>(_mapper.Map<PettyCashDto>(result), "Created successfully");



        }

        private async Task<Response<PettyCashDto>> Update(CreatePettyCashDto entity, int status)
        {
            if (entity.PettyCashLines.Count() == 0)
                return new Response<PettyCashDto>("Lines are required");

            var totalDebit = entity.PettyCashLines.Sum(i => i.Debit);

            var totalCredit = entity.PettyCashLines.Sum(i => i.Credit);

            foreach (var line in entity.PettyCashLines)
            {
                if (line.Debit > 0 && line.Credit > 0 || line.Credit == line.Debit)
                    return new Response<PettyCashDto>("Debit and Credit amount should be in seperate lines");
            }

            var specification = new PettyCashSpecs(true);
            var pettyCash = await _unitOfWork.PettyCash.GetById((int)entity.Id, specification);

            if (pettyCash == null)
                return new Response<PettyCashDto>("Not found");

            if (pettyCash.StatusId != 1 && pettyCash.StatusId != 2)
                return new Response<PettyCashDto>("Petty Cash already submitted");

            pettyCash.SetStatus(status);

            _unitOfWork.CreateTransaction();
            //For updating data
            _mapper.Map<CreatePettyCashDto, PettyCashMaster>(entity, pettyCash);

            await _unitOfWork.SaveAsync();

            //Commiting the transaction
            _unitOfWork.Commit();

            //returning response
            return new Response<PettyCashDto>(_mapper.Map<PettyCashDto>(pettyCash), "Updated successfully");
        }

        private async Task AddToLedger(PettyCashMaster pettyCash)
        {
            var transaction = new Transactions(pettyCash.Id, pettyCash.DocNo, DocType.PettyCash);
            var addTransaction = await _unitOfWork.Transaction.Add(transaction);
            await _unitOfWork.SaveAsync();

            pettyCash.SetTransactionId(transaction.Id);
            await _unitOfWork.SaveAsync();

            //Inserting data into recordledger table
            List<RecordLedger> recordLedger = pettyCash.PettyCashLines
                .Select(line => new RecordLedger(
                    transaction.Id,
                    line.AccountId,
                    line.BusinessPartnerId,
                    line.Description,
                    line.Debit > 0 && line.Credit <= 0 ? 'D' : 'C',
                    line.Debit > 0 && line.Credit <= 0 ? line.Debit : line.Credit,
                    pettyCash.CampusId,
                    line.Date
                    )).ToList();

            if (pettyCash.TotalDebit > 0)
            {
                //Credit Entry against Petty Cash Debit lines
                recordLedger.Add(new RecordLedger(
                    transaction.Id,
                    pettyCash.AccountId,
                    null,
                    pettyCash.Description,
                    'C',
                    pettyCash.TotalDebit,
                    pettyCash.CampusId,
                    pettyCash.Date
                ));
            }

            if (pettyCash.TotalCredit > 0)
            {
                //Debit Entry against Petty Cash Credit lines
                recordLedger.Add(new RecordLedger(
                    transaction.Id,
                    pettyCash.AccountId,
                    null,
                    pettyCash.Description,
                    'D',
                    pettyCash.TotalCredit,
                    pettyCash.CampusId,
                    pettyCash.Date
                ));
            }
            await _unitOfWork.Ledger.AddRange(recordLedger);
            await _unitOfWork.SaveAsync();
        }

        public async Task<Response<bool>> CheckWorkFlow(ApprovalDto data)
        {
            var getPettyCash = await _unitOfWork.PettyCash.GetById(data.DocId, new PettyCashSpecs(true));

            if (getPettyCash == null)
            {
                return new Response<bool>("Petty Cash with the input id not found");
            }
            if (getPettyCash.Status.State == DocumentStatus.Unpaid || getPettyCash.Status.State == DocumentStatus.Partial || getPettyCash.Status.State == DocumentStatus.Paid)
            {
                return new Response<bool>("Petty Cash already approved");
            }
            var workflow = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.PettyCash)).FirstOrDefault();

            if (workflow == null)
            {
                return new Response<bool>("No activated workflow found for this document");
            }
            var transition = workflow.WorkflowTransitions
                    .FirstOrDefault(x => (x.CurrentStatusId == getPettyCash.StatusId && x.Action == data.Action));

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
                    getPettyCash.SetStatus(transition.NextStatusId);
                    if (!String.IsNullOrEmpty(data.Remarks))
                    {
                        var addRemarks = new Remark()
                        {
                            DocId = getPettyCash.Id,
                            DocType = DocType.PettyCash,
                            Remarks = data.Remarks,
                            UserId = userId
                        };
                        await _unitOfWork.Remarks.Add(addRemarks);
                    }

                    if (transition.NextStatus.State == DocumentStatus.Unpaid)
                    {
                        await AddToLedger(getPettyCash);
                        _unitOfWork.Commit();
                        return new Response<bool>(true, "Petty Cash Approved");
                    }

                    if (transition.NextStatus.State == DocumentStatus.Rejected)
                    {
                        await _unitOfWork.SaveAsync();
                        _unitOfWork.Commit();
                        return new Response<bool>(true, "Petty Cash Rejected");
                    }
                    await _unitOfWork.SaveAsync();
                    _unitOfWork.Commit();

                    return new Response<bool>(true, "Petty Cash Reviewed");
                }
            }

            return new Response<bool>("User does not have allowed role");
        }

        private List<RemarksDto> ReturningRemarks(PettyCashDto data, DocType docType)
        {
            var remarks = _unitOfWork.Remarks.Find(new RemarksSpecs(data.Id, DocType.PettyCash))
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

        private List<FileUploadDto> ReturningFiles(PettyCashDto data, DocType docType)
        {

            var files = _unitOfWork.Fileupload.Find(new FileUploadSpecs(data.Id, DocType.PettyCash))
                    .Select(e => new FileUploadDto()
                    {
                        Id = e.Id,
                        Name = e.Name,
                        DocType = DocType.PettyCash,
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
