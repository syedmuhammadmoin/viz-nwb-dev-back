using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Specifications;
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

        public BudgetService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public BudgetService()
        {

        }
        public async Task<Response<BudgetDto>> CreateAsync(CreateBudgetDto entity)
        {
            if (entity.BudgetLines.Count() == 0)
                return new Response<BudgetDto>("Lines are required");
            
            if (entity.From > entity.To)
                return new Response<BudgetDto>("End date must be greater than Start Date");

            var budget = _mapper.Map<BudgetMaster>(entity);

            var checkExistingBudget = _unitOfWork.Budget.Find(new BudgetSpecs((DateTime)entity.From, budget.CampusId)).FirstOrDefault();
            if (checkExistingBudget != null)
                return new Response<BudgetDto>("Budget already consist in this span");
            

            var result = await _unitOfWork.Budget.Add(budget);
            await _unitOfWork.SaveAsync();
            return new Response<BudgetDto>(_mapper.Map<BudgetDto>(result), "Created successfully");
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

            return new Response<BudgetDto>(_mapper.Map<BudgetDto>(budget), "Returning value");
        }

        public async Task<Response<BudgetDto>> UpdateAsync(CreateBudgetDto entity)
        {
            var specification = new BudgetSpecs(true);
            var budget = await _unitOfWork.Budget.GetById((int)entity.Id, specification);

            if (budget == null)
                return new Response<BudgetDto>("Not found");

            if (entity.From > entity.To)
                return new Response<BudgetDto>("End date must be greater than Start Date");

            var checkExistingBudget = _unitOfWork.Budget.Find(new BudgetSpecs((DateTime)entity.From, budget.CampusId)).FirstOrDefault(i=> i.Id != entity.Id);
            if (checkExistingBudget != null)
                return new Response<BudgetDto>("Budget already consist in this span");
            
            if (entity.BudgetLines.Count() == 0)
                return new Response<BudgetDto>("Lines are required");

            //For updating data
            _mapper.Map<CreateBudgetDto, BudgetMaster>(entity, budget);
            await _unitOfWork.SaveAsync();
            return new Response<BudgetDto>(_mapper.Map<BudgetDto>(budget), "Updated successfully");

        }

        public async Task<Response<List<BudgetDto>>> GetBudgetDropDown()
        {
            var budgets = await _unitOfWork.Budget.GetAll();
            if (!budgets.Any())
                return new Response<List<BudgetDto>>("List is empty");

            return new Response<List<BudgetDto>>(_mapper.Map<List<BudgetDto>>(budgets), "Returning List");
        }

        public  Response<List<BudgetReportDto>> GetBudgetReport(BudgetReportFilters filters)
        {
            var getBudget = _unitOfWork.Budget.Find(new BudgetSpecs(filters.BudgetName))
                .FirstOrDefault();

            if (getBudget==null)
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
                    CampusId = i.Id,
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
                        BudgetAmount = budgetLine.Amount,
                        IncurredAmount = new BudgetService().getIncomeAccount(budgetLine.Account.Level1_id) ?
                        x.Sum(s => s.Credit - s.Debit) : x.Sum(s => s.Debit - s.Credit),
                        BalanceRemaining = budgetLine.Amount - (new BudgetService().getIncomeAccount(budgetLine.Account.Level1_id) ?
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
                        BudgetAmount = budgetLine.Amount,
                        BalanceRemaining = budgetLine.Amount - 0
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

        private bool getIncomeAccount(Guid id)
        {
            if (id == new Guid("40000000-5566-7788-99AA-BBCCDDEEFF00"))
            {
                return true;
            }
            return false;
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
