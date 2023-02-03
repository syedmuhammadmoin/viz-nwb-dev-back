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
    public class FixedAssetService : IFixedAssetService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public FixedAssetService(IUnitOfWork unitOfWork , IMapper mapper , IHttpContextAccessor httpContextAccessor) 
        {
         _mapper= mapper;
         _unitOfWork= unitOfWork;    
         _httpContextAccessor= httpContextAccessor;
        }

        public async Task<Response<FixedAssetDto>> CreateAsync(CreateFixedAssetDto entity)
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

        public async Task<PaginationResponse<List<FixedAssetDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var fixedAsset = await _unitOfWork.FixedAsset.GetAll(new FixedAssetSpecs(filter, false));

            if (fixedAsset.Count() == 0)
                return new PaginationResponse<List<FixedAssetDto>>(_mapper.Map<List<FixedAssetDto>>(fixedAsset), "List is empty");

            var totalRecords = await _unitOfWork.FixedAsset.TotalRecord(new FixedAssetSpecs(filter, true));

            return new PaginationResponse<List<FixedAssetDto>>(_mapper.Map<List<FixedAssetDto>>(fixedAsset),
                filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<FixedAssetDto>> GetByIdAsync(int id)
        {
            var fixedAsset = await _unitOfWork.FixedAsset.GetById(id, new FixedAssetSpecs());
            if (fixedAsset == null)
                return new Response<FixedAssetDto>("Not found");
            var fixedAssetDto = _mapper.Map<FixedAssetDto>(fixedAsset);

            if (fixedAssetDto.DepreciationApplicability == false)
            {
                fixedAssetDto.DepreciationId = null;
                fixedAssetDto.AccumulatedDepreciation = null;
                fixedAssetDto.AssetAccountId = null;
                fixedAssetDto.DepreciationExpenseId = null;
            }
            fixedAssetDto.IsAllowedRole = false;
            var workflow = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.FixedAsset)).FirstOrDefault();
            if (workflow != null)
            {
                var transition = workflow.WorkflowTransitions
                    .FirstOrDefault(x => (x.CurrentStatusId == fixedAssetDto.StatusId));

                if (transition != null)
                {
                    var currentUserRoles = new GetUser(this._httpContextAccessor).GetCurrentUserRoles();
                    foreach (var role in currentUserRoles)
                    {
                        if (transition.AllowedRole.Name == role)
                        {
                            fixedAssetDto.IsAllowedRole = true;
                        }
                    }
                }
            }
            return new Response<FixedAssetDto>(fixedAssetDto, "Returning value");
        }

        public async Task<Response<FixedAssetDto>> UpdateAsync(CreateFixedAssetDto entity)
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

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<bool>> CheckWorkFlow(ApprovalDto data)
        {
            var getFixedAsset = await _unitOfWork.FixedAsset.GetById(data.DocId, new FixedAssetSpecs());

            if (getFixedAsset == null)
            {
                return new Response<bool>("Fixed Asset with the input id not found");
            }
            if (getFixedAsset.Status.State == DocumentStatus.Unpaid || getFixedAsset.Status.State == DocumentStatus.Partial || getFixedAsset.Status.State == DocumentStatus.Paid)
            {
                return new Response<bool>("Fixed Asset already approved");
            }
            var workflow = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.FixedAsset)).FirstOrDefault();

            if (workflow == null)
            {
                return new Response<bool>("No activated workflow found for this document");
            }
            var transition = workflow.WorkflowTransitions
                    .FirstOrDefault(x => (x.CurrentStatusId == getFixedAsset.StatusId && x.Action == data.Action));

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
                    getFixedAsset.SetStatus(transition.NextStatusId);

                    if (!String.IsNullOrEmpty(data.Remarks))
                    {
                        var addRemarks = new Remark()
                        {
                            DocId = getFixedAsset.Id,
                            DocType = DocType.FixedAsset,
                            Remarks = data.Remarks,
                            UserId = userId
                        };
                        await _unitOfWork.Remarks.Add(addRemarks);
                    }

                    if (transition.NextStatus.State == DocumentStatus.Rejected)
                    {
                        await _unitOfWork.SaveAsync();
                        _unitOfWork.Commit();
                        return new Response<bool>(true, "FixedAsset Rejected");
                    }
                    await _unitOfWork.SaveAsync();
                    _unitOfWork.Commit();
                    return new Response<bool>(true, "FixedAsset Reviewed");
                }
            }

            return new Response<bool>("User does not have allowed role");


        }

        private async Task<Response<FixedAssetDto>> Submit(CreateFixedAssetDto entity)
        {
            var checkingActiveWorkFlows = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.FixedAsset)).FirstOrDefault();

            if (checkingActiveWorkFlows == null)
            {
                return new Response<FixedAssetDto>("No workflow found for Fixed Asset");
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

        private async Task<Response<FixedAssetDto>> Save(CreateFixedAssetDto entity, int status)
        {
            if (entity.DepreciationApplicability)
            {
                if (entity.DepreciationId == null && entity.DepreciationId == 0 || entity.DepreciationExpenseId == null ||
                    entity.UseFullLife == null)
                {
                    return new Response<FixedAssetDto>("Depreciation Model Fields are Required");
                }

                if (entity.ModelType == DepreciationMethod.Declining && entity.DecLiningRate == null)
                {
                    return new Response<FixedAssetDto>("Declining Rate is Required");
                }
            }
            else
            {
                entity.DepreciationApplicability = false;
                entity.DepreciationId = null;
                entity.AssetAccountId= null;
                entity.DepreciationExpenseId = null;
                entity.AccumulatedDepreciationId = null;
                entity.UseFullLife = null;
                entity.DecLiningRate = 0;
                entity.ModelType = 0 ;
            }

            var fix = _mapper.Map<FixedAsset>(entity);

            //Setting status
            fix.SetStatus(status);

            _unitOfWork.CreateTransaction();

            //Saving in table
            var result = await _unitOfWork.FixedAsset.Add(fix);
            await _unitOfWork.SaveAsync();

            //For creating docNo
            fix.CreateCode();
            await _unitOfWork.SaveAsync();

            //Commiting the transaction 
            _unitOfWork.Commit();

            //returning response
            return new Response<FixedAssetDto>(_mapper.Map<FixedAssetDto>(result), "Created successfully");

        }

        private async Task<Response<FixedAssetDto>> Update(CreateFixedAssetDto entity, int status)
        {
            if (entity.DepreciationApplicability)
            {
                if (entity.DepreciationId == null && entity.DepreciationId == 0 || entity.DepreciationExpenseId == null ||
                    entity.UseFullLife == null)
                {
                    return new Response<FixedAssetDto>("Depreciation Model Fields are Required");
                }

                if (entity.ModelType == DepreciationMethod.Declining && entity.DecLiningRate == null)
                {
                    return new Response<FixedAssetDto>("Declining Rate is Required");
                }
            }
            else
            {
                entity.DepreciationId = null;
                entity.DepreciationExpenseId = null;
                entity.AccumulatedDepreciationId = null;
                entity.AssetAccountId = null;
                entity.ModelType = 0 ;
                entity.UseFullLife = null;
                entity.DecLiningRate = 0;
            }

            var specification = new FixedAssetSpecs();
            var fix = await _unitOfWork.FixedAsset.GetById((int)entity.Id, specification);

            if (fix == null)
                return new Response<FixedAssetDto>("Not found");

            if (fix.StatusId != 1 && fix.StatusId != 2)
                return new Response<FixedAssetDto>("Only draft document can be edited");



            fix.SetStatus(status);

            _unitOfWork.CreateTransaction();

            //For updating data
            _mapper.Map<CreateFixedAssetDto, FixedAsset>(entity, fix);

            await _unitOfWork.SaveAsync();

            //Commiting the transaction
            _unitOfWork.Commit();

            //returning response
            return new Response<FixedAssetDto>(_mapper.Map<FixedAssetDto>(fix), "Updated successfully");

        }
    }
}
