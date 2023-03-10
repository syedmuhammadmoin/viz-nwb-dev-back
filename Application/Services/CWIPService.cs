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
    public class CWIPService : ICWIPService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public CWIPService(IUnitOfWork unitOfWork, IMapper mapper , IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Response<CWIPDto>> CreateAsync(CreateCWIPDto entity)
        {
            if ((bool)entity.IsSubmit)
            {
                return await this.Submit(entity);
            }
            else
            {
                return await this.Save(entity, 1);
            }
        }

        public async Task<Response<CWIPDto>> UpdateAsync(CreateCWIPDto entity)
        {
            if ((bool)entity.IsSubmit)
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
        
        public async Task<PaginationResponse<List<CWIPDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var result = await _unitOfWork.CWIP.GetAll(new CWIPSpecs(filter, false));

            if (result.Count() == 0)
                return new PaginationResponse<List<CWIPDto>>(_mapper.Map<List<CWIPDto>>(result), "List is empty");

            var totalRecords = await _unitOfWork.CWIP.TotalRecord(new CWIPSpecs(filter, true));

            return new PaginationResponse<List<CWIPDto>>(_mapper.Map<List<CWIPDto>>(result),
                filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<CWIPDto>> GetByIdAsync(int id)
        {
            var cwip = await _unitOfWork.CWIP.GetById(id, new CWIPSpecs());

            if (cwip == null)
                return new Response<CWIPDto>("Not found");

            var cwipDto = _mapper.Map<CWIPDto>(cwip);
            ReturningRemarks(cwipDto);

            if (cwipDto.DepreciationApplicability == false)
            {
                cwipDto.DepreciationModelId = null;
                cwipDto.AccumulatedDepreciation = null;
                cwipDto.DepreciationExpenseId = null;
                cwipDto.ModelType = DepreciationMethod.StraightLine;
                cwipDto.UseFullLife = null;
                cwipDto.DecLiningRate = 0;

            }

            cwipDto.IsAllowedRole = false;
            
            var workflow = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.CWIP)).FirstOrDefault();
            if (workflow != null)
            {
                var transition = workflow.WorkflowTransitions
                    .FirstOrDefault(x => (x.CurrentStatusId == cwipDto.StatusId));
                if (transition != null)
                {
                    var currentUserRoles = new GetUser(this._httpContextAccessor).GetCurrentUserRoles();
                    foreach (var role in currentUserRoles)
                    {
                        if (transition.AllowedRole.Name == role)
                        {
                            cwipDto.IsAllowedRole = true;
                        }
                    }
                }
            }
            return new Response<CWIPDto>(cwipDto, "Returning value");
        }
        
        public async Task<Response<bool>> CheckWorkFlow(ApprovalDto data)
        {
            var getCwip = await _unitOfWork.CWIP.GetById(data.DocId, new CWIPSpecs());

            if (getCwip == null)
            {
                return new Response<bool>("CWIP with the input id not found");
            }
            if (getCwip.Status.State == DocumentStatus.Unpaid || getCwip.Status.State == DocumentStatus.Partial || getCwip.Status.State == DocumentStatus.Paid)
            {
                return new Response<bool>("CWIP already approved");
            }
            var workflow = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.CWIP)).FirstOrDefault();

            if (workflow == null)
            {
                return new Response<bool>("No activated workflow found for this document");
            }
            var transition = workflow.WorkflowTransitions
                    .FirstOrDefault(x => (x.CurrentStatusId == getCwip.StatusId && x.Action == data.Action));

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
                    getCwip.SetStatus(transition.NextStatusId);
                    if (!String.IsNullOrEmpty(data.Remarks))
                    {
                        var addRemarks = new Remark()
                        {
                            DocId = getCwip.Id,
                            DocType = DocType.CWIP,
                            Remarks = data.Remarks,
                            UserId = userId
                        };
                        await _unitOfWork.Remarks.Add(addRemarks);
                    }

                    if (transition.NextStatus.State == DocumentStatus.Unpaid)
                    {
                        for (int i = 0; i < getCwip.Quantity; i++)
                        {
                            var fix = _mapper.Map<FixedAsset>(_mapper.Map<CreateFixedAssetDto>(getCwip));
                            //Setting status
                            fix.SetStatus(3);
                            await _unitOfWork.FixedAsset.Add(fix);
                            await _unitOfWork.SaveAsync();
                            //For creating docNo
                            fix.CreateCode();
                            await _unitOfWork.SaveAsync();
                        }
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
        private async Task<Response<CWIPDto>> Submit(CreateCWIPDto entity)
        {
            var checkingActiveWorkFlows = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.CWIP)).FirstOrDefault();

            if (checkingActiveWorkFlows == null)
            {
                return new Response<CWIPDto>("No workflow found for CWIP");
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

        private async Task<Response<CWIPDto>> Save(CreateCWIPDto entity, int status)
        {
            if (entity.SalvageValue > entity.Cost)
                return new Response<CWIPDto>("Salvage value cannot be greater than cost");
            if (entity.DepreciationApplicability)
            {
                if (entity.DepreciationModelId == null && entity.DepreciationModelId == 0 || entity.DepreciationExpenseId == null ||
                    entity.UseFullLife == null)
                {
                    return new Response<CWIPDto>("DepreciationModel Model Fields are Required");
                }

                if (entity.ModelType == DepreciationMethod.Declining && (entity.DecLiningRate == null || entity.DecLiningRate == 0))
                {
                    return new Response<CWIPDto>("Declining Rate is Required");
                }
            }
            else
            {
                entity.DepreciationModelId = null;
                entity.DepreciationExpenseId = null;
                entity.AccumulatedDepreciationId = null;
                entity.UseFullLife = null;
                entity.ModelType = DepreciationMethod.StraightLine;
                entity.DecLiningRate = 0;
            }

            var cwip = _mapper.Map<CWIP>(entity);
            _unitOfWork.CreateTransaction();
            
            //Setting status
            cwip.SetStatus(status);

            //Saving in table
            await _unitOfWork.CWIP.Add(cwip);
            await _unitOfWork.SaveAsync();

            //For creating docNo
            cwip.CreateCode();
            await _unitOfWork.SaveAsync();
            //Commiting the transaction 
            _unitOfWork.Commit();

            //returning response
            return new Response<CWIPDto>(_mapper.Map<CWIPDto>(cwip), "Created successfully");
        }

        private async Task<Response<CWIPDto>> Update(CreateCWIPDto entity , int status)
        {
            if (entity.SalvageValue > entity.Cost)
                return new Response<CWIPDto>("Salvage value cannot be greater than cost");
            if (entity.DepreciationApplicability)
            {
                if (entity.DepreciationModelId == null && entity.DepreciationModelId == 0 || entity.DepreciationExpenseId == null ||
                    entity.UseFullLife == null)
                {
                    return new Response<CWIPDto>("DepreciationModel Model Fields are Required");
                }

                if (entity.ModelType == DepreciationMethod.Declining && (entity.DecLiningRate == null || entity.DecLiningRate == 0))
                {
                    return new Response<CWIPDto>("Declining Rate is Required");
                }
            }
            else
            {
                entity.DepreciationModelId = null;
                entity.DepreciationExpenseId = null;
                entity.AccumulatedDepreciationId = null;
                entity.UseFullLife = null;
                entity.ModelType = DepreciationMethod.StraightLine;
                entity.DecLiningRate = 0;
            }

            var cwip = await _unitOfWork.CWIP.GetById((int)entity.Id);
            if (cwip == null)
                return new Response<CWIPDto>("Not found");

            //Setting status
            cwip.SetStatus(status);

            _unitOfWork.CreateTransaction();

            //For updating data
            _mapper.Map(entity, cwip);
            await _unitOfWork.SaveAsync();

            //Commiting the transaction
            _unitOfWork.Commit();

            //returning response
            return new Response<CWIPDto>(_mapper.Map<CWIPDto>(cwip), "Updated successfully");
        }

        private List<RemarksDto> ReturningRemarks(CWIPDto data)
        {
            var remarks = _unitOfWork.Remarks.Find(new RemarksSpecs(data.Id, DocType.CWIP))
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
