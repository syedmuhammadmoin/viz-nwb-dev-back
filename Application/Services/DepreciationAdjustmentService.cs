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
    public class DepreciationAdjustmentService : IDepreciationAdjustmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DepreciationAdjustmentService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Response<DepreciationAdjustmentDto>> CreateAsync(CreateDepreciationAdjustmentDto entity)
        {
            if ((bool)entity.IsSubmit)
            {
                return await SubmitDepreciationAdjustment(entity);
            }
            else
            {
                return await SaveDepreciationAdjustment(entity, 1);
            }
        }

        public async Task<PaginationResponse<List<DepreciationAdjustmentDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var depreciationAdjustment = await _unitOfWork.DepreciationAdjustment.GetAll(new DepreciationAdjustmentSpecs(filter, false));

            if (!depreciationAdjustment.Any())
                return new PaginationResponse<List<DepreciationAdjustmentDto>>(_mapper.Map<List<DepreciationAdjustmentDto>>(depreciationAdjustment), "List is empty");

            var totalRecords = await _unitOfWork.DepreciationAdjustment.TotalRecord(new DepreciationAdjustmentSpecs(filter, true));

            return new PaginationResponse<List<DepreciationAdjustmentDto>>(_mapper.Map<List<DepreciationAdjustmentDto>>(depreciationAdjustment),
                filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<DepreciationAdjustmentDto>> GetByIdAsync(int id)
        {
            var specification = new DepreciationAdjustmentSpecs(false);
            var depreciationAdjustment = await _unitOfWork.DepreciationAdjustment.GetById(id, specification);
            if (depreciationAdjustment == null)
                return new Response<DepreciationAdjustmentDto>("Not found");

            var depreciationAdjustmentDto = _mapper.Map<DepreciationAdjustmentDto>(depreciationAdjustment);

            ReturningRemarks(depreciationAdjustmentDto);

            depreciationAdjustmentDto.IsAllowedRole = false;
            var workflow = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.DepreciationAdjustment)).FirstOrDefault();

            if (workflow != null)
            {
                var transition = workflow.WorkflowTransitions
                    .FirstOrDefault(x => (x.CurrentStatusId == depreciationAdjustmentDto.StatusId));

                if (transition != null)
                {
                    var currentUserRoles = new GetUser(this._httpContextAccessor).GetCurrentUserRoles();
                    foreach (var role in currentUserRoles)
                    {
                        if (transition.AllowedRole.Name == role)
                        {
                            depreciationAdjustmentDto.IsAllowedRole = true;
                        }
                    }
                }
            }
            return new Response<DepreciationAdjustmentDto>(depreciationAdjustmentDto, "Returning value");
        }

        public async Task<Response<DepreciationAdjustmentDto>> UpdateAsync(CreateDepreciationAdjustmentDto entity)
        {
            if ((bool)entity.IsSubmit)
            {
                return await SubmitDepreciationAdjustment(entity);
            }
            else
            {
                return await UpdateDepreciationAdjustment(entity, 1);
            }
        }

        private List<RemarksDto> ReturningRemarks(DepreciationAdjustmentDto data)
        {
            var remarks = _unitOfWork.Remarks.Find(new RemarksSpecs(data.Id, DocType.DepreciationAdjustment))
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
        //Private
        private async Task<Response<DepreciationAdjustmentDto>> SubmitDepreciationAdjustment(CreateDepreciationAdjustmentDto entity)
        {
            var checkingActiveWorkFlows = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.DepreciationAdjustment)).FirstOrDefault();

            if (checkingActiveWorkFlows == null)
            {
                return new Response<DepreciationAdjustmentDto>("No workflow found for Depreciation Adjustment");
            }

            if (entity.Id == null)
            {
                return await SaveDepreciationAdjustment(entity, 6);
            }
            else
            {
                return await UpdateDepreciationAdjustment(entity, 6);
            }
        }
        private async Task<Response<DepreciationAdjustmentDto>> SaveDepreciationAdjustment(CreateDepreciationAdjustmentDto entity, int status)
        {
            if (entity.DepreciationAdjustmentLines.Count() == 0)
                return new Response<DepreciationAdjustmentDto>("Lines are required");

            var depreciationAdjustment = _mapper.Map<DepreciationAdjustmentMaster>(entity);

            //Setting status
            depreciationAdjustment.SetStatus(status);

            //_unitOfWork.CreateTransaction();

            //Saving in table
            var result = await _unitOfWork.DepreciationAdjustment.Add(depreciationAdjustment);
            await _unitOfWork.SaveAsync();

            ////For creating docNo
            //depreciationAdjustment.CreateDocNo();
            //await _unitOfWork.SaveAsync();

            ////Commiting the transaction 
            //_unitOfWork.Commit();

            //returning response
            return new Response<DepreciationAdjustmentDto>(_mapper.Map<DepreciationAdjustmentDto>(result), "Created successfully");

        }
        private async Task<Response<DepreciationAdjustmentDto>> UpdateDepreciationAdjustment(CreateDepreciationAdjustmentDto entity, int status)
        {
            if (entity.DepreciationAdjustmentLines.Count() == 0)
                return new Response<DepreciationAdjustmentDto>("Lines are required");

            var specification = new DepreciationAdjustmentSpecs(true);
            var depreciationAdjustment = await _unitOfWork.DepreciationAdjustment.GetById((int)entity.Id, specification);

            if (depreciationAdjustment == null)
                return new Response<DepreciationAdjustmentDto>("Not found");

            if (depreciationAdjustment.StatusId != 1 && depreciationAdjustment.StatusId != 2)
                return new Response<DepreciationAdjustmentDto>("Only draft document can be edited");

            depreciationAdjustment.SetStatus(status);

            _unitOfWork.CreateTransaction();

            //For updating data
            _mapper.Map(entity, depreciationAdjustment);

            await _unitOfWork.SaveAsync();

            //Commiting the transaction
            _unitOfWork.Commit();

            //returning response
            return new Response<DepreciationAdjustmentDto>(_mapper.Map<DepreciationAdjustmentDto>(depreciationAdjustment), "Created successfully");

        }
        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
        public async Task<Response<bool>> CheckWorkFlow(ApprovalDto data)
        {
            var getDepreciationAdjustment = await _unitOfWork.DepreciationAdjustment.GetById(data.DocId, new DepreciationAdjustmentSpecs(true));

            if (getDepreciationAdjustment  == null)
            {
                return new Response<bool>("Depreciation Adjustment with the input id not found");
            }
            if (getDepreciationAdjustment.Status.State == DocumentStatus.Unpaid || getDepreciationAdjustment.Status.State == DocumentStatus.Partial || getDepreciationAdjustment.Status.State == DocumentStatus.Paid)
            {
                return new Response<bool>("Depreciation Adjustment already approved");
            }
            var workflow = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.DepreciationAdjustment)).FirstOrDefault();

            if (workflow == null)
            {
                return new Response<bool>("No activated workflow found for this document");
            }
            var transition = workflow.WorkflowTransitions
                    .FirstOrDefault(x => (x.CurrentStatusId == getDepreciationAdjustment.StatusId && x.Action == data.Action));

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
                    getDepreciationAdjustment.SetStatus(transition.NextStatusId);
                    if (!String.IsNullOrEmpty(data.Remarks))
                    {
                        var addRemarks = new Remark()
                        {
                            DocId = getDepreciationAdjustment.Id,
                            DocType = DocType.DepreciationAdjustment,
                            Remarks = data.Remarks,
                            UserId = userId
                        };
                        await _unitOfWork.Remarks.Add(addRemarks);
                    }


                    if (transition.NextStatus.State == DocumentStatus.Unpaid)
                    {
                        return new Response<bool>(true, "Depreciation Adjustment Approved");
                    }
                    if (transition.NextStatus.State == DocumentStatus.Rejected)
                    {
                        await _unitOfWork.SaveAsync();
                        _unitOfWork.Commit();
                        return new Response<bool>(true, "Depreciation Adjustment Rejected");
                    }
                    await _unitOfWork.SaveAsync();
                    _unitOfWork.Commit();
                    return new Response<bool>(true, "Depreciation Adjustment Reviewed");
                }
            }

            return new Response<bool>("User does not have allowed role");
        }
    }
}
