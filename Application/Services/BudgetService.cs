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

        public async Task<Response<BudgetDto>> CreateAsync(CreateBudgetDto entity)
        {
            if (entity.BudgetLines.Count() == 0)
                return new Response<BudgetDto>("Lines are required");
            
            if (entity.From > entity.To)
                return new Response<BudgetDto>("End date must be greater than Start Date");

            var budget = _mapper.Map<BudgetMaster>(entity);

            var checkExistingBudget = _unitOfWork.Budget.Find(new BudgetSpecs(entity.From)).FirstOrDefault();
            if (checkExistingBudget != null)
                return new Response<BudgetDto>("Budget already consist in this span");
            

            var result = await _unitOfWork.Budget.Add(budget);
            await _unitOfWork.SaveAsync();
            return new Response<BudgetDto>(_mapper.Map<BudgetDto>(result), "Created successfully");
        }

        public async Task<PaginationResponse<List<BudgetDto>>> GetAllAsync(PaginationFilter filter)
        {
            var specification = new BudgetSpecs(filter);
            var budgets = await _unitOfWork.Budget.GetAll(specification);

            if (!budgets.Any())
                return new PaginationResponse<List<BudgetDto>>("List is empty");

            var totalRecords = await _unitOfWork.Budget.TotalRecord();

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

            var checkExistingBudget = _unitOfWork.Budget.Find(new BudgetSpecs(entity.From)).FirstOrDefault(i=> i.Id != entity.Id);
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

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
