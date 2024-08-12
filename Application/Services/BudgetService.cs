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
    public class BudgetService : IBudgetService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public BudgetService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        public BudgetService()
        {

        }
        public async Task<Response<BudgetDto>> CreateAsync(CreateBudgetDto entity)
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

        public async Task<PaginationResponse<List<BudgetDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var budgets = await _unitOfWork.Budget.GetAll(new BudgetSpecs(filter, false));

            if (!budgets.Any())
                return new PaginationResponse<List<BudgetDto>>(_mapper.Map<List<BudgetDto>>(budgets), "List is empty");

            var totalRecords = await _unitOfWork.Budget.TotalRecord(new BudgetSpecs(filter, true));

            return new PaginationResponse<List<BudgetDto>>(_mapper.Map<List<BudgetDto>>(budgets),
                filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<BudgetDto>> GetByIdAsync(int id)
        {
            var specification = new BudgetSpecs(false);
            var budget = await _unitOfWork.Budget.GetById(id, specification);
            if (budget == null)
                return new Response<BudgetDto>("Not found");

            //Calling general ledger view
            var generalLedger = _unitOfWork.Ledger.Find(new LedgerSpecs())
                .Select(i => new GeneralLedgerDto()
                {
                    LedgerId = i.Id,
                    AccountId = i.Level4_id,
                    AccountName = i.Level4.Name,
                    TransactionId = i.Id,
                    CampusId = i.CampusId,
                    DocDate = i.TransactionDate,
                    DocType = i.Transactions.DocType,
                    DocNo = i.Transactions.DocNo,
                    Description = i.Description,
                    Debit = i.Sign == 'D' ? i.Amount : 0,
                    Credit = i.Sign == 'C' ? i.Amount : 0,
                    BId = i.BusinessPartnerId,
                    BusinessPartnerName = i.BusinessPartner != null ? i.BusinessPartner.Name : "N/A",
                    WarehouseName = i.Warehouse != null ? i.Warehouse.Name : "N/A",
                    CampusName = i.Campus != null ? i.Campus.Name : "N/A",
                    Balance = i.Sign == 'D' ? i.Amount : (-1) * i.Amount
                });

            decimal currentTotal = 0;
            //Applying Union
            var generalLedgerView = generalLedger
                .OrderBy(e => e.LedgerId)
                .ThenBy(e => e.DocDate)
                .ThenBy(e => e.TransactionId)
                .Select(gl =>
                {
                    currentTotal += (gl.Debit - gl.Credit);
                    gl.Balance = currentTotal;
                    return gl;
                });

            var genLedger = generalLedgerView.ToList();
            BudgetDto budgetDto = new BudgetDto();

            budgetDto = _mapper.Map<BudgetDto>(budget);

            budgetDto.BudgetLines.Clear(); 

            foreach (var budgetLine in budget.BudgetLines)
            {

                //Getting data for the given data range
                var glWithBudgetFilter = genLedger
                    .Where(e => (e.AccountId == budgetLine.AccountId) &&
                    ((e.DocDate >= budget.From && e.DocDate <= budget.To)
                    && budget.CampusId == e.CampusId))
                    .OrderBy(x => x.DocDate)
                    .ThenBy(x => x.TransactionId)
                    .GroupBy(x => x.AccountId)
                .Select(x => new BudgetLinesDto()
                {
                    AccountId = x.Key,
                    IncurredAmount = new BudgetService().getIncomeAccount(budgetLine.Account.Level1_id) ?
                    x.Sum(s => s.Credit - s.Debit) : x.Sum(s => s.Debit - s.Credit),
                }).FirstOrDefault();
                if (glWithBudgetFilter!=null)
                {
                    glWithBudgetFilter.Id = budgetLine.Id;
                    glWithBudgetFilter.Amount = budgetLine.Amount;
                    glWithBudgetFilter.RevisedAmount = budgetLine.RevisedAmount;
                    glWithBudgetFilter.AccountName = budgetLine.Account.Name;
                    glWithBudgetFilter.MasterId = budgetLine.MasterId;
                    
                    budgetDto.BudgetLines.Add(glWithBudgetFilter);
                }
                else
                {
                    var budgetLinetoSave = _mapper.Map<BudgetLinesDto>(budgetLine);
                    budgetDto.BudgetLines.Add(budgetLinetoSave);
                }


            }

            if (budgetDto.State == DocumentStatus.Unpaid || budgetDto.State == DocumentStatus.Partial || budgetDto.State == DocumentStatus.Paid)
            {
                return new Response<BudgetDto>(budgetDto, "Returning value");
            }

            budgetDto.IsAllowedRole = false;
            var workflow = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.Budget)).FirstOrDefault();
            if (workflow != null)
            {
                var transition = workflow.WorkflowTransitions
                    .FirstOrDefault(x => (x.CurrentStatusId == budgetDto.StatusId));

                if (transition != null)
                {
                    var currentUserRoles = new GetUser(this._httpContextAccessor).GetCurrentUserRoles();
                    foreach (var role in currentUserRoles)
                    {
                        if (transition.AllowedRole.Name == role)
                        {
                            budgetDto.IsAllowedRole = true;
                        }
                    }
                }
            }


            return new Response<BudgetDto>(budgetDto, "Returning value");
        }

        public async Task<Response<BudgetDto>> UpdateAsync(CreateBudgetDto entity)
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

        public async Task<Response<List<BudgetDto>>> GetBudgetDropDown()
        {
            var budgets = await _unitOfWork.Budget.GetAll();
            if (!budgets.Any())
                return new Response<List<BudgetDto>>("List is empty");

            return new Response<List<BudgetDto>>(_mapper.Map<List<BudgetDto>>(budgets), "Returning List");
        }

        public Response<List<BudgetReportDto>> GetBudgetReport(BudgetReportFilters filters)
        {
            var getBudget = _unitOfWork.Budget.Find(new BudgetSpecs(filters.BudgetName))
                .FirstOrDefault();

            if (getBudget == null)
                return new Response<List<BudgetReportDto>>("Budget not found");

            if (filters.To > getBudget.To)
                return new Response<List<BudgetReportDto>>("Till date is greater than budget end date");

            if (filters.To < getBudget.From)
                return new Response<List<BudgetReportDto>>("Till date is lesser than budget end date");

            //Calling general ledger view
            var generalLedger = _unitOfWork.Ledger.Find(new LedgerSpecs())
                .Select(i => new GeneralLedgerDto()
                {
                    LedgerId = i.Id,
                    AccountId = i.Level4_id,
                    AccountName = i.Level4.Name,
                    TransactionId = i.Id,
                    CampusId = i.CampusId,
                    DocDate = i.TransactionDate,
                    DocType = i.Transactions.DocType,
                    DocNo = i.Transactions.DocNo,
                    Description = i.Description,
                    Debit = i.Sign == 'D' ? i.Amount : 0,
                    Credit = i.Sign == 'C' ? i.Amount : 0,
                    BId = i.BusinessPartnerId,
                    BusinessPartnerName = i.BusinessPartner != null ? i.BusinessPartner.Name : "N/A",
                    WarehouseName = i.Warehouse != null ? i.Warehouse.Name : "N/A",
                    CampusName = i.Campus != null ? i.Campus.Name : "N/A",
                    Balance = i.Sign == 'D' ? i.Amount : (-1) * i.Amount
                });

            decimal currentTotal = 0;
            //Applying Union
            var generalLedgerView = generalLedger
                .OrderBy(e => e.LedgerId)
                .ThenBy(e => e.DocDate)
                .ThenBy(e => e.TransactionId)
                .Select(gl =>
                {
                    currentTotal += (gl.Debit - gl.Credit);
                    gl.Balance = currentTotal;
                    return gl;
                });

            var genLedger = generalLedgerView.ToList();
            List<BudgetReportDto> result = new List<BudgetReportDto>();

            foreach (var budgetLine in getBudget.BudgetLines)
            {

                //Getting data for the given data range
                var glWithBudgetFilter = genLedger
                    .Where(e => (e.AccountId == budgetLine.AccountId) &&
                    ((e.DocDate >= getBudget.From && e.DocDate <= filters.To)
                    && getBudget.CampusId == e.CampusId))
                    .OrderBy(x => x.DocDate)
                    .ThenBy(x => x.TransactionId)
                    .GroupBy(x => x.AccountId)
                    .Select(x => new BudgetReportDto()
                    {
                        BudgetId = getBudget.Id,
                        BudgetName = getBudget.BudgetName,
                        CampusId = getBudget.CampusId,
                        CampusName = getBudget.Campus.Name,
                        From = getBudget.From,
                        To = filters.To,
                        AccountId = budgetLine.AccountId,
                        Account = budgetLine.Account.Name,
                        BudgetAmount = budgetLine.RevisedAmount,
                        IncurredAmount = new BudgetService().getIncomeAccount(budgetLine.Account.Level1_id) ?
                        x.Sum(s => s.Credit - s.Debit) : x.Sum(s => s.Debit - s.Credit),
                        BalanceRemaining = budgetLine.RevisedAmount - (new BudgetService().getIncomeAccount(budgetLine.Account.Level1_id) ?
                        x.Sum(s => s.Credit - s.Debit) : x.Sum(s => s.Debit - s.Credit))
                    }).FirstOrDefault();

                if (glWithBudgetFilter == null)
                {
                    var budgetWithoutEntry = new BudgetReportDto()
                    {
                        BudgetId = getBudget.Id,
                        BudgetName = getBudget.BudgetName,
                        CampusId = getBudget.CampusId,
                        CampusName = getBudget.Campus.Name,
                        From = getBudget.From,
                        To = filters.To,
                        AccountId = budgetLine.AccountId,
                        Account = budgetLine.Account.Name,
                        BudgetAmount = budgetLine.RevisedAmount,
                        BalanceRemaining = budgetLine.RevisedAmount - 0
                    };

                    result.Add(budgetWithoutEntry);
                }
                else
                {
                    result.Add(glWithBudgetFilter);
                }
            }

            return new Response<List<BudgetReportDto>>(result, "Returning budget report");
        }

        private bool getIncomeAccount(string id)
        {
             string IncomeAccountLevel1 = "40000000-5566-7788-99AA-BBCCDDEEFF00" + $"-{GetTenant.GetTenantId(_httpContextAccessor)}";

            if (id == IncomeAccountLevel1)
            {
                return true;
            }
            return false;
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
        //Private Methods 
        private async Task<Response<BudgetDto>> Save(CreateBudgetDto entity, int status)
        {
            if (entity.BudgetLines.Count() == 0)
                return new Response<BudgetDto>("Lines are required");

            var budget = _mapper.Map<BudgetMaster>(entity);

            var checkExistingBudget = _unitOfWork.Budget.Find(new BudgetSpecs((DateTime)entity.From, budget.CampusId)).FirstOrDefault();
            if (checkExistingBudget != null)
                return new Response<BudgetDto>("Budget already consist in this span");


            budget.SetStatus(status);
            var result = await _unitOfWork.Budget.Add(budget);
            await _unitOfWork.SaveAsync();
            return new Response<BudgetDto>(_mapper.Map<BudgetDto>(result), "Created successfully");
        }
        private async Task<Response<BudgetDto>> Update(CreateBudgetDto entity, int status)
        {

            if (entity.BudgetLines.Count() == 0)
                return new Response<BudgetDto>("Lines are required");

            if (entity.From > entity.To)
                return new Response<BudgetDto>("End date must be greater than Start Date");

            var specification = new BudgetSpecs(true);
            var budget = await _unitOfWork.Budget.GetById((int)entity.Id, specification);

            if (budget == null)
                return new Response<BudgetDto>("Not found");

            if (budget.StatusId != 1 && budget.StatusId != 2)
                return new Response<BudgetDto>("Only draft document can be edited");

            var checkExistingBudget = _unitOfWork.Budget.Find(new BudgetSpecs((DateTime)entity.From, budget.CampusId)).FirstOrDefault(i => i.Id != entity.Id);
            if (checkExistingBudget != null)
                return new Response<BudgetDto>("Budget already consist in this span");



            budget.SetStatus(status);


            _mapper.Map<CreateBudgetDto, BudgetMaster>(entity, budget);

            await _unitOfWork.SaveAsync();


            return new Response<BudgetDto>(_mapper.Map<BudgetDto>(budget), "Updated successfully");

        }
        private async Task<Response<BudgetDto>> Submit(CreateBudgetDto entity)
        {
            var checkingActiveWorkFlows = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.Budget)).FirstOrDefault();

            if (checkingActiveWorkFlows == null)
            {
                return new Response<BudgetDto>("No workflow found for Budget");
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
        public async Task<Response<bool>> CheckWorkFlow(ApprovalDto data)
        {
            var budget = await _unitOfWork.Budget.GetById(data.DocId, new BudgetSpecs(false));

            if (budget == null)
            {
                return new Response<bool>("Budget with the input id not found");
            }

            if (budget.Status.State == DocumentStatus.Unpaid || budget.Status.State == DocumentStatus.Partial || budget.Status.State == DocumentStatus.Paid)
            {
                return new Response<bool>("Budget already approved");
            }

            var workflow = _unitOfWork.WorkFlow
                .Find(new WorkFlowSpecs(DocType.Budget))
                .FirstOrDefault();

            if (workflow == null)
            {
                return new Response<bool>("No activated workflow found for this document");
            }

            var transition = workflow.WorkflowTransitions
                    .FirstOrDefault(x => (x.CurrentStatusId == budget.StatusId && x.Action == data.Action));

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
                    budget.SetStatus(transition.NextStatusId);
                    if (!String.IsNullOrEmpty(data.Remarks))
                    {
                        var addRemarks = new Remark()
                        {
                            DocId = budget.Id,
                            DocType = DocType.Budget,
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
    }
}
