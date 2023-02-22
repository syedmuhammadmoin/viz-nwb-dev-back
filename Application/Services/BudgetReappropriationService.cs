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
    public class BudgetReappropriationService : IBudgetReappropriationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BudgetReappropriationService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Response<BudgetReappropriationDto>> CreateAsync(CreateBudgetReappropriationDto entity)
        {
            if ((bool)entity.IsSubmit)
            {
                return await SubmitBudgetReappropriation(entity);
            }
            else
            {
                return await SaveBudgetReappropriation(entity, 1);
            }
        }

        public async Task<PaginationResponse<List<BudgetReappropriationDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var budgetReappropriations = await _unitOfWork.BudgetReappropriation.GetAll(new BudgetReappropriationSpecs(filter, false));

            if (!budgetReappropriations.Any())
                return new PaginationResponse<List<BudgetReappropriationDto>>(_mapper.Map<List<BudgetReappropriationDto>>(budgetReappropriations), "List is empty");

            var totalRecords = await _unitOfWork.BudgetReappropriation.TotalRecord(new BudgetReappropriationSpecs(filter, true));

            return new PaginationResponse<List<BudgetReappropriationDto>>(_mapper.Map<List<BudgetReappropriationDto>>(budgetReappropriations),
                filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<BudgetReappropriationDto>> GetByIdAsync(int id)
        {
            var specification = new BudgetReappropriationSpecs(false);
            var budgetReappropriation = await _unitOfWork.BudgetReappropriation.GetById(id, specification);
            if (budgetReappropriation == null)
                return new Response<BudgetReappropriationDto>("Not found");

            var budgetReappropriationDto = _mapper.Map<BudgetReappropriationDto>(budgetReappropriation);

            ReturningRemarks(budgetReappropriationDto);

            budgetReappropriationDto.IsAllowedRole = false;
            var workflow = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.BudgetReappropriation)).FirstOrDefault();

            if (workflow != null)
            {
                var transition = workflow.WorkflowTransitions
                    .FirstOrDefault(x => (x.CurrentStatusId == budgetReappropriationDto.StatusId));

                if (transition != null)
                {
                    var currentUserRoles = new GetUser(this._httpContextAccessor).GetCurrentUserRoles();
                    foreach (var role in currentUserRoles)
                    {
                        if (transition.AllowedRole.Name == role)
                        {
                            budgetReappropriationDto.IsAllowedRole = true;
                        }
                    }
                }
            }
            return new Response<BudgetReappropriationDto>(budgetReappropriationDto, "Returning value");
        }

        public async Task<Response<List<BudgetReappropriationDto>>> GetDropDown()
        {
            var budgets = await _unitOfWork.BudgetReappropriation.GetAll();
            if (!budgets.Any())
                return new Response<List<BudgetReappropriationDto>>("List is empty");

            return new Response<List<BudgetReappropriationDto>>(_mapper.Map<List<BudgetReappropriationDto>>(budgets), "Returning List");
        }
        private async Task<Response<BudgetReappropriationDto>> SubmitBudgetReappropriation(CreateBudgetReappropriationDto entity)
        {
            var checkingActiveWorkFlows = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.BudgetReappropriation)).FirstOrDefault();

            if (checkingActiveWorkFlows == null)
            {
                return new Response<BudgetReappropriationDto>("No workflow found for BudgetReappropriation");
            }

            if (entity.Id == null)
            {
                return await this.SaveBudgetReappropriation(entity, 6);
            }
            else
            {
                return await this.UpdateBudgetReappropriation(entity, 6);
            }
        }

        public async Task<Response<BudgetReappropriationDto>> UpdateAsync(CreateBudgetReappropriationDto entity)
        {
            if ((bool)entity.IsSubmit)
            {
                return await SubmitBudgetReappropriation(entity);
            }
            else
            {
                return await UpdateBudgetReappropriation(entity, 1);
            }
        }

        private async Task<Response<BudgetReappropriationDto>> SaveBudgetReappropriation(CreateBudgetReappropriationDto entity, int status)
        {
            if (entity.BudgetReappropriationLines.Count() == 0)
                return new Response<BudgetReappropriationDto>("Lines are required");

            var budgetReappropriation = _mapper.Map<BudgetReappropriationMaster>(entity);

            //Setting status
            budgetReappropriation.SetStatus(status);

            _unitOfWork.CreateTransaction();

            //Saving in table
            var result = await _unitOfWork.BudgetReappropriation.Add(budgetReappropriation);
            await _unitOfWork.SaveAsync();

            ////For creating docNo
            //budgetReappropriation.CreateDocNo();
            //await _unitOfWork.SaveAsync();

            //Commiting the transaction 
            _unitOfWork.Commit();

            //returning response
            return new Response<BudgetReappropriationDto>(_mapper.Map<BudgetReappropriationDto>(result), "Created successfully");

        }

        private async Task<Response<BudgetReappropriationDto>> UpdateBudgetReappropriation(CreateBudgetReappropriationDto entity, int status)
        {
            if (entity.BudgetReappropriationLines.Count() == 0)
                return new Response<BudgetReappropriationDto>("Lines are required");

            var specification = new BudgetReappropriationSpecs(true);
            var budgetReappropriation = await _unitOfWork.BudgetReappropriation.GetById((int)entity.Id, specification);

            if (budgetReappropriation == null)
                return new Response<BudgetReappropriationDto>("Not found");

            if (budgetReappropriation.StatusId != 1 && budgetReappropriation.StatusId != 2)
                return new Response<BudgetReappropriationDto>("Only draft document can be edited");

            budgetReappropriation.SetStatus(status);

            _unitOfWork.CreateTransaction();

            //For updating data
            _mapper.Map<CreateBudgetReappropriationDto, BudgetReappropriationMaster>(entity, budgetReappropriation);

            await _unitOfWork.SaveAsync();

            //Commiting the transaction
            _unitOfWork.Commit();

            //returning response
            return new Response<BudgetReappropriationDto>(_mapper.Map<BudgetReappropriationDto>(budgetReappropriation), "Created successfully");

        }
        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<bool>> CheckWorkFlow(ApprovalDto data)
        {
            var getBudgetReappropriation = await _unitOfWork.BudgetReappropriation.GetById(data.DocId, new BudgetReappropriationSpecs(true));

            if (getBudgetReappropriation == null)
            {
                return new Response<bool>("BudgetReappropriation with the input id not found");
            }
            if (getBudgetReappropriation.Status.State == DocumentStatus.Unpaid || getBudgetReappropriation.Status.State == DocumentStatus.Partial || getBudgetReappropriation.Status.State == DocumentStatus.Paid)
            {
                return new Response<bool>("BudgetReappropriation already approved");
            }
            var workflow = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.BudgetReappropriation)).FirstOrDefault();

            if (workflow == null)
            {
                return new Response<bool>("No activated workflow found for this document");
            }
            var transition = workflow.WorkflowTransitions
                    .FirstOrDefault(x => (x.CurrentStatusId == getBudgetReappropriation.StatusId && x.Action == data.Action));

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
                    getBudgetReappropriation.SetStatus(transition.NextStatusId);
                    if (!String.IsNullOrEmpty(data.Remarks))
                    {
                        var addRemarks = new Remark()
                        {
                            DocId = getBudgetReappropriation.Id,
                            DocType = DocType.BudgetReappropriation,
                            Remarks = data.Remarks,
                            UserId = userId
                        };
                        await _unitOfWork.Remarks.Add(addRemarks);
                    }


                    if (transition.NextStatus.State == DocumentStatus.Unpaid)
                    {
                        return new Response<bool>(true, "BudgetReappropriation Approved");
                    }
                    if (transition.NextStatus.State == DocumentStatus.Rejected)
                    {
                        await _unitOfWork.SaveAsync();
                        _unitOfWork.Commit();
                        return new Response<bool>(true, "BudgetReappropriation Rejected");
                    }
                    await _unitOfWork.SaveAsync();
                    _unitOfWork.Commit();
                    return new Response<bool>(true, "BudgetReappropriation Reviewed");
                }
            }

            return new Response<bool>("User does not have allowed role");
        }

        private List<RemarksDto> ReturningRemarks(BudgetReappropriationDto data)
        {
            var remarks = _unitOfWork.Remarks.Find(new RemarksSpecs(data.Id, DocType.BudgetReappropriation))
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
