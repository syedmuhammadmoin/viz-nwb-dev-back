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

namespace Application.Services
{
    public class FixedAssetService : IFixedAssetService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FixedAssetService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Response<FixedAssetDto>> CreateAsync(CreateFixedAssetDto entity)
        {
            //Checking workflow
            int status = 1;
            if ((bool)entity.IsSubmit)
            {
                var checkingActiveWorkFlows = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.FixedAsset)).FirstOrDefault();
                if (checkingActiveWorkFlows == null)
                {
                    return new Response<FixedAssetDto>("No workflow found for Fixed Asset");
                }
                status = 6;
            }

            //Checking validation
            if (entity.DepreciationApplicability)
            {
                if ((entity.DepreciationModelId == null && entity.DepreciationModelId == 0)
                    || (entity.UseFullLife == null && entity.UseFullLife == 0)
                    || entity.DepreciationExpenseId == null)
                {
                    return new Response<FixedAssetDto>("Depreciation Model Fields are Required");
                }
                if (entity.ModelType == DepreciationMethod.Declining && (entity.DecLiningRate == null || entity.DecLiningRate == 0))
                {
                    return new Response<FixedAssetDto>("Declining Rate is Required");
                }
            }
            else
            {
                entity.DepreciationModelId = null;
                entity.AssetAccountId = null;
                entity.DepreciationExpenseId = null;
                entity.AccumulatedDepreciationId = null;
                entity.UseFullLife = null;
                entity.DecLiningRate = 0;
                entity.ModelType = 0;
            }

            _unitOfWork.CreateTransaction();
            if( entity.Quantity == 0)
            {
                return new Response<FixedAssetDto>("Quantity is Required");
            }
            for (int i = 0; i < entity.Quantity; i++)
            {
                var fix = _mapper.Map<FixedAsset>(entity);
                //Setting status
                fix.SetStatus(status);
                await _unitOfWork.FixedAsset.Add(fix);
                await _unitOfWork.SaveAsync();
                //For creating docNo
                fix.CreateCode();
                await _unitOfWork.SaveAsync();
            }

            if (entity.DocId!=null)
            {

                if (entity.DocId != null && entity.Doctype == DocType.GRN)
                {
                    var GRNLines =  _unitOfWork.GRN
                        .FindLines(new GRNLinesSpecs((int)entity.DocId , (int)entity.ProductId, (int)entity.WarehouseId, false))
                        .FirstOrDefault();
                  
                    if(GRNLines != null )
                    {
                        GRNLines.SetIsFixedAssetCreatedTrue();
                        await _unitOfWork.SaveAsync();
                    }
                } 
            }

            //Commiting the transaction 
            _unitOfWork.Commit();

            //returning response
            return new Response<FixedAssetDto>(null, "Created successfully");
        }

        public async Task<Response<FixedAssetDto>> UpdateAsync(UpdateFixedAssetDto entity)
        {
            //Checking workflow
            int status = 1;
            if ((bool)entity.IsSubmit)
            {
                var checkingActiveWorkFlows = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.FixedAsset)).FirstOrDefault();
                if (checkingActiveWorkFlows == null)
                {
                    return new Response<FixedAssetDto>("No workflow found for Fixed Asset");
                }
                status = 6;
            }

            //Checking validation
            if (entity.DepreciationApplicability)
            {
                if ((entity.DepreciationModelId == null && entity.DepreciationModelId == 0)
                    || (entity.UseFullLife == null && entity.UseFullLife == 0)
                    || entity.DepreciationExpenseId == null)
                {
                    return new Response<FixedAssetDto>("Depreciation Model Fields are Required");
                }
                if (entity.ModelType == DepreciationMethod.Declining && (entity.DecLiningRate == null || entity.DecLiningRate == 0))
                {
                    return new Response<FixedAssetDto>("Declining Rate is Required");
                }
            }
            else
            {
                entity.DepreciationModelId = null;
                entity.AssetAccountId = null;
                entity.DepreciationExpenseId = null;
                entity.AccumulatedDepreciationId = null;
                entity.UseFullLife = null;
                entity.DecLiningRate = 0;
                entity.ModelType = 0;
            }

            //Getting fixed asset
            var result = await _unitOfWork.FixedAsset.GetById((int)entity.Id);
            if (result == null)
                return new Response<FixedAssetDto>("Not found");

            if (result.StatusId != 1 && result.StatusId != 2)
                return new Response<FixedAssetDto>("Only draft document can be edited");

            //Setting status
            result.SetStatus(status);

            _unitOfWork.CreateTransaction();
            //For updating data
            _mapper.Map(entity, result);
            await _unitOfWork.SaveAsync();

            //Commiting the transaction
            _unitOfWork.Commit();

            //returning response
            return new Response<FixedAssetDto>(_mapper.Map<FixedAssetDto>(result), "Updated successfully");
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
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
            ReturningRemarks(fixedAssetDto);

            if (fixedAssetDto.DepreciationApplicability == false)
            {
                fixedAssetDto.DepreciationModelId = null;
                fixedAssetDto.AssetAccountId = null;
                fixedAssetDto.AccumulatedDepreciation = null;
                fixedAssetDto.DepreciationExpenseId = null;
            }

            fixedAssetDto.IsAllowedRole = false;

            var workflow = _unitOfWork.WorkFlow
                .Find(new WorkFlowSpecs(DocType.FixedAsset))
                .FirstOrDefault();

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

        public async Task<Response<List<FixedAssetDto>>> GetDropDown()
        {
            var fixedAsset = await _unitOfWork.FixedAsset.GetAll();
            if (!fixedAsset.Any())
                return new Response<List<FixedAssetDto>>(null, "List is empty");

            return new Response<List<FixedAssetDto>>(_mapper.Map<List<FixedAssetDto>>(fixedAsset), "Returning List");

        }

        public async Task<Response<List<FixedAssetDto>>> GetDisposableAssetDropDown()
        {
            var fixedAsset = await _unitOfWork.FixedAsset.GetAll(new FixedAssetSpecs(true));
            if (!fixedAsset.Any())
                return new Response<List<FixedAssetDto>>(null, "List is empty");

            return new Response<List<FixedAssetDto>>(_mapper.Map<List<FixedAssetDto>>(fixedAsset), "Returning List");

        }

        public async Task<Response<List<FixedAssetDto>>> GetAssetByProductIdDropDown(int ProductId)
        {
            var fixedAsset = await _unitOfWork.FixedAsset.GetAll(new FixedAssetSpecs(ProductId));
            if (!fixedAsset.Any())
                return new Response<List<FixedAssetDto>>(null, "List is empty");

            return new Response<List<FixedAssetDto>>(_mapper.Map<List<FixedAssetDto>>(fixedAsset), "Returning List");

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

            var workflow = _unitOfWork.WorkFlow
                .Find(new WorkFlowSpecs(DocType.FixedAsset))
                .FirstOrDefault();

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

                    if (transition.NextStatus.State == DocumentStatus.Unpaid)
                    {
                        await _unitOfWork.SaveAsync();
                        _unitOfWork.Commit();
                        return new Response<bool>(true, "Document Approved");
                    }
                    if (transition.NextStatus.State == DocumentStatus.Rejected)
                    {
                        await _unitOfWork.SaveAsync();
                        _unitOfWork.Commit();
                        return new Response<bool>(true, "Document Rejected");
                    }
                    await _unitOfWork.SaveAsync();
                    _unitOfWork.Commit();
                    return new Response<bool>(true, "Document Reviewed");
                }
            }

            return new Response<bool>("User does not have allowed role");


        }

        //Private methods
        private List<RemarksDto> ReturningRemarks(FixedAssetDto data)
        {
            var remarks = _unitOfWork.Remarks.Find(new RemarksSpecs(data.Id, DocType.FixedAsset))
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

    }
}
