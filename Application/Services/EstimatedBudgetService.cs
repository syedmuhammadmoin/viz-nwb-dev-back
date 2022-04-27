using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using AutoMapper;
using Domain.Constants;
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
    public class EstimatedBudgetService : IEstimatedBudgetService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EstimatedBudgetService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<EstimatedBudgetDto>> CreateAsync(CreateEstimatedBudgetDto entity)
        {
            if(entity.EstimatedBudgetLines.Count() == 0)
                return new Response<EstimatedBudgetDto>("Lines are required");

            var estimatedBudget = _mapper.Map<EstimatedBudgetMaster>(entity);

            var result = await _unitOfWork.EstimatedBudget.Add(estimatedBudget);
            await _unitOfWork.SaveAsync();
            return new Response<EstimatedBudgetDto>(_mapper.Map<EstimatedBudgetDto>(result), "Created successfully");
        }

        public async Task<PaginationResponse<List<EstimatedBudgetDto>>> GetAllAsync(PaginationFilter filter)
        {
            var specification = new EstimatedBudgetSpecs(filter);
            var estimatedEstimatedBudgets = await _unitOfWork.EstimatedBudget.GetAll(specification);

            if (!estimatedEstimatedBudgets.Any())
                return new PaginationResponse<List<EstimatedBudgetDto>>("List is empty");

            var totalRecords = await _unitOfWork.EstimatedBudget.TotalRecord();

            return new PaginationResponse<List<EstimatedBudgetDto>>(_mapper.Map<List<EstimatedBudgetDto>>(estimatedEstimatedBudgets),
                filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<EstimatedBudgetDto>> GetByIdAsync(int id)
        {
            var specification = new EstimatedBudgetSpecs(false);
            var estimatedBudget = await _unitOfWork.EstimatedBudget.GetById(id, specification);
            if (estimatedBudget == null)
                return new Response<EstimatedBudgetDto>("Not found");

            return new Response<EstimatedBudgetDto>(_mapper.Map<EstimatedBudgetDto>(estimatedBudget), "Returning value");
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
            var specification = new EstimatedBudgetSpecs(true);
            var estimatedBudget = await _unitOfWork.EstimatedBudget.GetById((int)entity.Id, specification);

            if (estimatedBudget == null)
                return new Response<EstimatedBudgetDto>("Not found");

            if (entity.EstimatedBudgetLines.Count() == 0)
                return new Response<EstimatedBudgetDto>("Lines are required");

            //For updating data
            _mapper.Map<CreateEstimatedBudgetDto, EstimatedBudgetMaster>(entity, estimatedBudget);
            await _unitOfWork.SaveAsync();
            return new Response<EstimatedBudgetDto>(_mapper.Map<EstimatedBudgetDto>(estimatedBudget), "Updated successfully");
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
