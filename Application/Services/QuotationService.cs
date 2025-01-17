﻿using Application.Contracts.DTOs;
using Application.Contracts.DTOs.Quotation;
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
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class QuotationService : IQuotationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public QuotationService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<PaginationResponse<List<QuotationDto>>> GetAllAsync(TransactionFormFilter filter)
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

            var quotation = await _unitOfWork.Quotation.GetAll(new QuotationSpecs(docDate, states, filter, false));

            if (quotation.Count() == 0)
            {
                return new PaginationResponse<List<QuotationDto>>(_mapper.Map<List<QuotationDto>>(quotation), "List is empty");
            }
            var totalRecords = await _unitOfWork.Quotation.TotalRecord(new QuotationSpecs(docDate, states, filter, true));

            return new PaginationResponse<List<QuotationDto>>(_mapper.Map<List<QuotationDto>>(quotation),
                    filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<QuotationDto>> GetByIdAsync(int id)
        {
            var quotation = await _unitOfWork.Quotation.GetById(id, new QuotationSpecs(false));
            if (quotation == null)
                return new Response<QuotationDto>("Not found");

            var quotationDto = _mapper.Map<QuotationDto>(quotation);
            //Add Reference
            if (quotationDto.RequisitionId != null)
            {
                quotationDto.References = new List<ReferncesDto> {
                    new ReferncesDto()
                    {
                        DocId = (int)quotationDto.RequisitionId,
                        DocNo = "REQ-" + String.Format("{0:000}", quotationDto.RequisitionId),
                        DocType = DocType.Requisition
                    }
                };
            }
            ReturningRemarks(quotationDto, DocType.Quotation);
            ReturningFiles(quotationDto, DocType.Quotation);

            if ((quotationDto.State == DocumentStatus.Unpaid || quotationDto.State == DocumentStatus.Partial || quotationDto.State == DocumentStatus.Paid))
                return new Response<QuotationDto>(quotationDto, "Returning value");           
            quotationDto.IsAllowedRole = false;
            var workflow = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.Quotation)).FirstOrDefault();
            if (workflow != null)
            {
                var transition = workflow.WorkflowTransitions
                    .FirstOrDefault(x => (x.CurrentStatusId == quotationDto.StatusId));
                if (transition != null)
                {
                    var currentUserRoles = new GetUser(this._httpContextAccessor).GetCurrentUserRoles();
                    foreach (var role in currentUserRoles)
                    {
                        if (transition.AllowedRole.Name == role)
                        {
                            quotationDto.IsAllowedRole = true;
                        }
                    }
                }
            }
            return new Response<QuotationDto>(quotationDto, "Returning value");
        }

        public async Task<Response<List<QuotationDto>>> GetQoutationByRequisitionId(GetQouteByReqDto data)
        {
            var quotation = await _unitOfWork.Quotation.GetAll(new QuotationSpecs(data.RequisitionId, data.QuotationCompId));
            if (quotation.Count() == 0)
                return new Response<List<QuotationDto>>(_mapper.Map<List<QuotationDto>>(quotation), "List is Empty");

            var quotationDto = _mapper.Map<List<QuotationDto>>(quotation);
            return new Response<List<QuotationDto>>(quotationDto, "Returning value");
        }
        
        public async Task<Response<QuotationDto>> CreateAsync(CreateQuotationDto entity)
        {
            if ((bool)entity.isSubmit)
            {
                return await SubmitQuotation(entity);
            }
            else
            {
                return await SaveQuotation(entity, 1);
            }
        }

        public async Task<Response<QuotationDto>> UpdateAsync(CreateQuotationDto entity)
        {
            if ((bool)entity.isSubmit)
            {
                return await SubmitQuotation(entity);
            }
            else
            {
                return await UpdateQuotation(entity, 1);
            }
        }
        
        public async Task<Response<bool>> CheckWorkFlow(ApprovalDto data)
        {
            var getQuottion = await _unitOfWork.Quotation.GetById(data.DocId, new QuotationSpecs(true));
            if (getQuottion == null)
                return new Response<bool>("Quottion with the input id not found");
            
            if (getQuottion.Status.State == DocumentStatus.Unpaid || getQuottion.Status.State == DocumentStatus.Partial || getQuottion.Status.State == DocumentStatus.Paid)
                return new Response<bool>("Quottion already approved");
            
            var workflow = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.Quotation)).FirstOrDefault();
            if (workflow == null)
                return new Response<bool>("No activated workflow found for this document");
            
            var transition = workflow.WorkflowTransitions
                  .FirstOrDefault(x => (x.CurrentStatusId == getQuottion.StatusId && x.Action == data.Action));
            if (transition == null)
                return new Response<bool>("No transition found");
            
            var getUser = new GetUser(this._httpContextAccessor);
            var userId = getUser.GetCurrentUserId();
            var currentUserRoles = new GetUser(this._httpContextAccessor).GetCurrentUserRoles();
            
            _unitOfWork.CreateTransaction();
            foreach (var role in currentUserRoles)
            {
                if (transition.AllowedRole.Name == role)
                {
                    getQuottion.SetStatus(transition.NextStatusId);
                    if (!String.IsNullOrEmpty(data.Remarks))
                    {
                        var addRemarks = new Remark()
                        {
                            DocId = getQuottion.Id,
                            DocType = DocType.Quotation,
                            Remarks = data.Remarks,
                            UserId = userId
                        };
                        await _unitOfWork.Remarks.Add(addRemarks);
                    }
                    if (transition.NextStatus.State == DocumentStatus.Unpaid)
                    {
                        await _unitOfWork.SaveAsync();
                        _unitOfWork.Commit();
                        return new Response<bool>(true, "Quottion Approved");
                    }
                    if (transition.NextStatus.State == DocumentStatus.Rejected)
                    {
                        await _unitOfWork.SaveAsync();
                        _unitOfWork.Commit();
                        return new Response<bool>(true, "Quottion Rejected");
                    }
                    await _unitOfWork.SaveAsync();
                    _unitOfWork.Commit();
                    return new Response<bool>(true, "Quottion Reviewed");
                }
            }
            return new Response<bool>("User does not have allowed role");
        }
        
        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
        
        
        private async Task<Response<QuotationDto>> SubmitQuotation(CreateQuotationDto entity)
        {
            var checkingActiveWorkFlows = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.Quotation)).FirstOrDefault();
            if (checkingActiveWorkFlows == null)
                return new Response<QuotationDto>("No workflow found for Quottion");

            if (entity.Id == null)
            {
                return await SaveQuotation(entity, 6);
            }
            else
            {
                return await UpdateQuotation(entity, 6);
            }
        }
        
        private async Task<Response<QuotationDto>> UpdateQuotation(CreateQuotationDto entity, int status)
        {
            if (entity.QuotationLines.Count() == 0)
                return new Response<QuotationDto>("Lines are required");

            var quotation = await _unitOfWork.Quotation.GetById((int)entity.Id, new QuotationSpecs(true));
            if (quotation == null)
                return new Response<QuotationDto>("Not found");

            if (quotation.StatusId != 1 && quotation.StatusId != 2)
                return new Response<QuotationDto>("Only draft document can be edited");

            quotation.SetStatus(status);

            //For updating data
            _mapper.Map<CreateQuotationDto, QuotationMaster>(entity, quotation);
            
            _unitOfWork.CreateTransaction();

            await _unitOfWork.SaveAsync();
            //Commiting the transaction
            _unitOfWork.Commit();

            //returning response
            return new Response<QuotationDto>(_mapper.Map<QuotationDto>(quotation), "Updated successfully");
        }
        
        private async Task<Response<QuotationDto>> SaveQuotation(CreateQuotationDto entity, int status)
        {
            if (entity.QuotationLines.Count() == 0)
                return new Response<QuotationDto>("Lines are Required");

            var quotation = _mapper.Map<QuotationMaster>(entity);

            //Setting status
            quotation.SetStatus(status);

            _unitOfWork.CreateTransaction();
            
            //Saving in table
            var result = await _unitOfWork.Quotation.Add(quotation);
            await _unitOfWork.SaveAsync();

            //For creating docNo
            quotation.CreateDocNo();
            await _unitOfWork.SaveAsync();

            //Commiting the transaction 
            _unitOfWork.Commit();
            return new Response<QuotationDto>(_mapper.Map<QuotationDto>(result), "Created successfully");
        }
        
        private List<RemarksDto> ReturningRemarks(QuotationDto data, DocType docType)
        {
            var remarks = _unitOfWork.Remarks.Find(new RemarksSpecs(data.Id, DocType.Quotation))
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
        
        private List<FileUploadDto> ReturningFiles(QuotationDto data, DocType docType)
        {

            var files = _unitOfWork.Fileupload.Find(new FileUploadSpecs(data.Id, DocType.Quotation))
                    .Select(e => new FileUploadDto()
                    {
                        Id = e.Id,
                        Name = e.Name,
                        DocType = DocType.Quotation,
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
