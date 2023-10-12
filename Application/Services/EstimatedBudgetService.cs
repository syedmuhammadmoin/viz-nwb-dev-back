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
using OfficeOpenXml.FormulaParsing.Excel.Functions.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class EstimatedBudgetService : IEstimatedBudgetService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public EstimatedBudgetService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Response<EstimatedBudgetDto>> CreateAsync(CreateEstimatedBudgetDto entity)
        {

            if ((bool)entity.isSubmit)
            {
                return await this.Submit(entity);
            }
            else
            {
                return await this.Save(entity,1);
            }

            
        }
        //Private Methods 
        private async Task<Response<EstimatedBudgetDto>> Save(CreateEstimatedBudgetDto entity, int status)
        {
            if (entity.EstimatedBudgetLines.Count() == 0)
                return new Response<EstimatedBudgetDto>("Lines are required");

            var estimatedBudget = _mapper.Map<EstimatedBudgetMaster>(entity);
            estimatedBudget.SetStatus(status);
            var result = await _unitOfWork.EstimatedBudget.Add(estimatedBudget);
            await _unitOfWork.SaveAsync();
            return new Response<EstimatedBudgetDto>(_mapper.Map<EstimatedBudgetDto>(result), "Created successfully");
        }
        private async Task<Response<EstimatedBudgetDto>> Update(CreateEstimatedBudgetDto entity, int status)
        {
            if (entity.EstimatedBudgetLines.Count() == 0)
                return new Response<EstimatedBudgetDto>("Lines are required");

            var specification = new EstimatedBudgetSpecs(true);
            var estimatedBudget = await _unitOfWork.EstimatedBudget.GetById((int)entity.Id, specification);

            if (estimatedBudget == null)
                return new Response<EstimatedBudgetDto>("Not found");

            if (estimatedBudget.StatusId != 1 && estimatedBudget.StatusId != 2)
                return new Response<EstimatedBudgetDto>("Only draft document can be edited");

            estimatedBudget.SetStatus(status);


            _mapper.Map<CreateEstimatedBudgetDto, EstimatedBudgetMaster>(entity, estimatedBudget);
          
            await _unitOfWork.SaveAsync();


            return new Response<EstimatedBudgetDto>(_mapper.Map<EstimatedBudgetDto>(estimatedBudget), "Updated successfully");

        }
        private async Task<Response<EstimatedBudgetDto>> Submit(CreateEstimatedBudgetDto entity)
        {
            var checkingActiveWorkFlows = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.EstimatedBudget)).FirstOrDefault();

            if (checkingActiveWorkFlows == null)
            {
                return new Response<EstimatedBudgetDto>("No workflow found for Estimated Budget");
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


        public async Task<PaginationResponse<List<EstimatedBudgetDto>>> GetAllAsync(TransactionFormFilter filter)
        {

            var estimatedEstimatedBudgets = await _unitOfWork.EstimatedBudget.GetAll(new EstimatedBudgetSpecs(filter, false));

            if (!estimatedEstimatedBudgets.Any())
                return new PaginationResponse<List<EstimatedBudgetDto>>(_mapper.Map<List<EstimatedBudgetDto>>(estimatedEstimatedBudgets), "List is empty");

            var totalRecords = await _unitOfWork.EstimatedBudget.TotalRecord(new EstimatedBudgetSpecs(filter, true));

            return new PaginationResponse<List<EstimatedBudgetDto>>(_mapper.Map<List<EstimatedBudgetDto>>(estimatedEstimatedBudgets),
                filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<EstimatedBudgetDto>> GetByIdAsync(int id)
        {
            var specification = new EstimatedBudgetSpecs(false);
            var estimatedBudget = await _unitOfWork.EstimatedBudget.GetById(id, specification);
            if (estimatedBudget == null)
                return new Response<EstimatedBudgetDto>("Not found");

            var estimatedBudgetDto = _mapper.Map<EstimatedBudgetDto>(estimatedBudget);

            if (estimatedBudgetDto.State == DocumentStatus.Unpaid || estimatedBudgetDto.State == DocumentStatus.Partial || estimatedBudgetDto.State == DocumentStatus.Paid)
            {
                return new Response<EstimatedBudgetDto>(estimatedBudgetDto, "Returning value");
            }

            estimatedBudgetDto.IsAllowedRole = false;
            var workflow = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.EstimatedBudget)).FirstOrDefault();
            if (workflow != null)
            {
                var transition = workflow.WorkflowTransitions
                    .FirstOrDefault(x => (x.CurrentStatusId == estimatedBudgetDto.StatusId));

                if (transition != null)
                {
                    var currentUserRoles = new GetUser(this._httpContextAccessor).GetCurrentUserRoles();
                    foreach (var role in currentUserRoles)
                    {
                        if (transition.AllowedRole.Name == role)
                        {
                            estimatedBudgetDto.IsAllowedRole = true;
                        }
                    }
                }
            }

            return new Response<EstimatedBudgetDto>(estimatedBudgetDto, "Returning value");
        }

        public async Task<Response<List<EstimatedBudgetDto>>> GetEstimatedBudgetDropDown()
        {
            var estimatedEstimatedBudgets = await _unitOfWork.EstimatedBudget.GetAll();
            if (!estimatedEstimatedBudgets.Any())
                return new Response<List<EstimatedBudgetDto>>("List is empty");

            return new Response<List<EstimatedBudgetDto>>(_mapper.Map<List<EstimatedBudgetDto>>(estimatedEstimatedBudgets), "Returning List");
        }

        public async Task<Response<EstimatedBudgetDto>> UpdateAsync(CreateEstimatedBudgetDto entity)
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
            var getEstimatedBudget = await _unitOfWork.EstimatedBudget.GetById(data.DocId, new EstimatedBudgetSpecs(false));

            if (getEstimatedBudget == null)
            {
                return new Response<bool>("Estimated Budget with the input id not found");
            }

            if (getEstimatedBudget.Status.State == DocumentStatus.Unpaid || getEstimatedBudget.Status.State == DocumentStatus.Partial || getEstimatedBudget.Status.State == DocumentStatus.Paid)
            {
                return new Response<bool>("Estimated Budget already approved");
            }

            var workflow = _unitOfWork.WorkFlow
                .Find(new WorkFlowSpecs(DocType.EstimatedBudget))
                .FirstOrDefault();

            if (workflow == null)
            {
                return new Response<bool>("No activated workflow found for this document");
            }

            var transition = workflow.WorkflowTransitions
                    .FirstOrDefault(x => (x.CurrentStatusId == getEstimatedBudget.StatusId && x.Action == data.Action));

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
                    getEstimatedBudget.SetStatus(transition.NextStatusId);
                    if (!String.IsNullOrEmpty(data.Remarks))
                    {
                        var addRemarks = new Remark()
                        {
                            DocId = getEstimatedBudget.Id,
                            DocType = DocType.FixedAsset,
                            Remarks = data.Remarks,
                            UserId = userId
                        };
                        await _unitOfWork.Remarks.Add(addRemarks);
                    }

                    if (transition.NextStatus.State == DocumentStatus.Unpaid)
                    {
                        //var budget = _mapper.Map<BudgetMaster>(_mapper.Map<CreateBudgetDto>(getEstimatedBudget));
                        //await _unitOfWork.Budget.Add(budget);
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
    }
}
