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
    public class RequestService : IRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RequestService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
 
        public async Task<Response<RequestDto>> CreateAsync(CreateRequestDto entity)
        {
            if ((bool)entity.isSubmit)
            {
                return await this.SubmitRequest(entity);
            }
            else
            {
                return await this.SaveRequest(entity, 1);
            }
        }
        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
        public async Task<PaginationResponse<List<RequestDto>>> GetAllAsync(TransactionFormFilter filter)
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

            var request = await _unitOfWork.Request.GetAll(new RequestSpecs(docDate, states, filter, false));

            if (request.Count() == 0)
            {
                return new PaginationResponse<List<RequestDto>>(_mapper.Map<List<RequestDto>>(request), "List is empty");
            }
            var totalRecords = await _unitOfWork.Request.TotalRecord(new RequestSpecs(docDate, states, filter, true));

            return new PaginationResponse<List<RequestDto>>(_mapper.Map<List<RequestDto>>(request),
                    filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }
        public async Task<Response<RequestDto>> GetByIdAsync(int id)
        {
            var specification = new RequestSpecs(false);
            var request = await _unitOfWork.Request.GetById(id, specification);
            if (request == null)
                return new Response<RequestDto>("Not found");

            var requestDto = _mapper.Map<RequestDto>(request);
            ReturningRemarks(requestDto, DocType.Request);

            ReturningFiles(requestDto, DocType.Request);

            if ((requestDto.State == DocumentStatus.Partial || requestDto.State == DocumentStatus.Paid))
            {
                return new Response<RequestDto>(requestDto, "Returning value");
            }
            requestDto.IsAllowedRole = false;
            var workflow = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.Request)).FirstOrDefault();
            if (workflow != null)
            {
                var transition = workflow.WorkflowTransitions
                    .FirstOrDefault(x => (x.CurrentStatusId == requestDto.StatusId));

                if (transition != null)
                {
                    var currentUserRoles = new GetUser(this._httpContextAccessor).GetCurrentUserRoles();
                    foreach (var role in currentUserRoles)
                    {
                        if (transition.AllowedRole.Name == role)
                        {
                            requestDto.IsAllowedRole = true;
                        }
                    }
                }
            }

            return new Response<RequestDto>(requestDto, "Returning value");
        }
        public async Task<Response<RequestDto>> UpdateAsync(CreateRequestDto entity)
        {
            if ((bool)entity.isSubmit)
            {
                return await this.SubmitRequest(entity);
            }
            else
            {
                return await this.UpdateRequest(entity, 1);
            }
        }
        public async Task<Response<bool>> CheckWorkFlow(ApprovalDto data)
        {
            var getRequest = await _unitOfWork.Request.GetById(data.DocId, new RequestSpecs(true));

            if (getRequest == null)
            {
                return new Response<bool>("Request with the input id not found");
            }
            if (getRequest.Status.State == DocumentStatus.Unpaid || getRequest.Status.State == DocumentStatus.Partial || getRequest.Status.State == DocumentStatus.Paid)
            {
                return new Response<bool>("Request already approved");
            }
            var workflow = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.Request)).FirstOrDefault();

            if (workflow == null)
            {
                return new Response<bool>("No activated workflow found for this document");
            }
            var transition = workflow.WorkflowTransitions
                  .FirstOrDefault(x => (x.CurrentStatusId == getRequest.StatusId && x.Action == data.Action));

            if (transition == null)
            {
                return new Response<bool>("No transition found");
            }
            var getUser = new GetUser(this._httpContextAccessor);
            var userId = getUser.GetCurrentUserId();
            var currentUserRoles = new GetUser(this._httpContextAccessor).GetCurrentUserRoles();
            _unitOfWork.CreateTransaction();

            foreach (var role in currentUserRoles)
            {
                if (transition.AllowedRole.Name == role)
                {
                    getRequest.setStatus(transition.NextStatusId);
                    if (!String.IsNullOrEmpty(data.Remarks))
                    {
                        var addRemarks = new Remark()
                        {
                            DocId = getRequest.Id,
                            DocType = DocType.Request,
                            Remarks = data.Remarks,
                            UserId = userId
                        };
                        await _unitOfWork.Remarks.Add(addRemarks);
                    }
                    if (transition.NextStatus.State == DocumentStatus.Unpaid)
                    {
                        await _unitOfWork.SaveAsync();
                        _unitOfWork.Commit();
                        return new Response<bool>(true, "Request Approved");
                    }
                    if (transition.NextStatus.State == DocumentStatus.Rejected)
                    {
                        await _unitOfWork.SaveAsync();
                        _unitOfWork.Commit();
                        return new Response<bool>(true, "Request Rejected");
                    }
                    await _unitOfWork.SaveAsync();
                    _unitOfWork.Commit();
                    return new Response<bool>(true, "Request Reviewed");
                }
            }

            return new Response<bool>("User does not have allowed role");
        }

        private async Task<Response<RequestDto>> SubmitRequest(CreateRequestDto entity)
        {
            var checkingActiveWorkFlows = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.Request)).FirstOrDefault();

            if (checkingActiveWorkFlows == null)
            {
                return new Response<RequestDto>("No workflow found for Request");
            }

            if (entity.Id == null)
            {
                return await this.SaveRequest(entity, 6);
            }
            else
            {
                return await this.UpdateRequest(entity, 6);
            }
        }
        private async Task<Response<RequestDto>> SaveRequest(CreateRequestDto entity, int status)
        {
            if (entity.RequestLines.Count() == 0)
                return new Response<RequestDto>("Lines are Required");

            var request = _mapper.Map<RequestMaster>(entity);

            //Setting status
            request.setStatus(status);

            _unitOfWork.CreateTransaction();

            //Saving in table
            var result = await _unitOfWork.Request.Add(request);
            await _unitOfWork.SaveAsync();

            //For creating docNo
            request.CreateDocNo();
            await _unitOfWork.SaveAsync();

            //Commiting the transaction 
            _unitOfWork.Commit();

            return new Response<RequestDto>(_mapper.Map<RequestDto>(result), "Created successfully");
        }
        private async Task<Response<RequestDto>> UpdateRequest(CreateRequestDto entity, int status)
        {
            if (entity.RequestLines.Count() == 0)
                return new Response<RequestDto>("Lines are required");

            var specification = new RequestSpecs(true);
            var request = await _unitOfWork.Request.GetById((int)entity.Id, specification);

            if (request == null)
                return new Response<RequestDto>("Not found");

            if (request.StatusId != 1 && request.StatusId != 2)
                return new Response<RequestDto>("Only draft document can be edited");

            request.setStatus(status);

            _unitOfWork.CreateTransaction();

            //For updating data
            _mapper.Map<CreateRequestDto, RequestMaster>(entity, request);

            await _unitOfWork.SaveAsync();

            //Commiting the transaction
            _unitOfWork.Commit();

            //returning response
            return new Response<RequestDto>(_mapper.Map<RequestDto>(request), "Updated successfully");

        }

        private List<RemarksDto> ReturningRemarks(RequestDto data, DocType docType)
        {
            var remarks = _unitOfWork.Remarks.Find(new RemarksSpecs(data.Id, DocType.Request))
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
        private List<FileUploadDto> ReturningFiles(RequestDto data, DocType docType)
        {

            var files = _unitOfWork.Fileupload.Find(new FileUploadSpecs(data.Id, DocType.Request))
                    .Select(e => new FileUploadDto()
                    {
                        Id = e.Id,
                        Name = e.Name,
                        DocType = DocType.Request,
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
