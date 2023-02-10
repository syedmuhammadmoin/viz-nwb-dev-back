using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Helper;
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
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Response<CategoryDto>> CreateAsync(CreateCategoryDto entity)
        {
            var category = _mapper.Map<Category>(entity);

            //Validation for same Accounts
            if (entity.InventoryAccountId == entity.CostAccountId || entity.InventoryAccountId == entity.RevenueAccountId || entity.CostAccountId == entity.RevenueAccountId)
            {
                return new Response<CategoryDto>("Account Cannot Be Same");
            }
            
            //Validation for Payable and Receivable
            var Inventorylevel4 = await _unitOfWork.Level4.GetById((Guid)entity.InventoryAccountId);

            var InventoryAccountId = ReceivableAndPayable.Validate(Inventorylevel4.Level3_id);

            if (InventoryAccountId == false)
            {
                return new Response<CategoryDto>("Inventory account Invalid");
            }

            var Revenuelevel4 = await _unitOfWork.Level4.GetById((Guid)entity.RevenueAccountId);

            var RevenueAccountId = ReceivableAndPayable.Validate(Revenuelevel4.Level3_id);

            if (RevenueAccountId == false)
            {
                return new Response<CategoryDto>("Revenue account Invalid");
            }
            var Costlevel4 = await _unitOfWork.Level4.GetById((Guid)entity.CostAccountId);

            var CostAccountId = ReceivableAndPayable.Validate(Costlevel4.Level3_id);

            if (CostAccountId == false)
            {
                return new Response<CategoryDto>("Cost account Invalid");
            }

            if ((bool)entity.IsFixedAsset && (entity.DepreciationId == null || entity.DepreciationId == 0))
            {
                return new Response<CategoryDto>("Depreciation is Required");
            }
            if ((bool)!entity.IsFixedAsset)
            {
                category.DepreciationIdnull();
            }
            var result = await _unitOfWork.Category.Add(category);
            await _unitOfWork.SaveAsync();

            return new Response<CategoryDto>(_mapper.Map<CategoryDto>(result), "Created successfully");
        }

        public async Task<PaginationResponse<List<CategoryDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var category = await _unitOfWork.Category.GetAll(new CategorySpecs(filter, false));

            if (category.Count() == 0)
                return new PaginationResponse<List<CategoryDto>>(_mapper.Map<List<CategoryDto>>(category), "List is empty");

            var totalRecords = await _unitOfWork.Category.TotalRecord(new CategorySpecs(filter, true));

            return new PaginationResponse<List<CategoryDto>>(_mapper.Map<List<CategoryDto>>(category), filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<CategoryDto>> GetByIdAsync(int id)
        {

            var specification = new CategorySpecs();
            var category = await _unitOfWork.Category.GetById(id, specification);
            if (category == null)
                return new Response<CategoryDto>("Not found");

            return new Response<CategoryDto>(_mapper.Map<CategoryDto>(category), "Returning value");
        }

        public async Task<Response<CategoryDto>> UpdateAsync(CreateCategoryDto entity)
        {
            var category = await _unitOfWork.Category.GetById((int)entity.Id);

            if (category == null)
                return new Response<CategoryDto>("Not found");

            //Validation for same Accounts
            if (entity.InventoryAccountId == entity.CostAccountId || entity.InventoryAccountId == entity.RevenueAccountId || entity.CostAccountId == entity.RevenueAccountId)
            {
                return new Response<CategoryDto>("Account Cannot Be Same");
            }

            //Validation for Payable and Receivable
            var Inventorylevel4 = await _unitOfWork.Level4.GetById((Guid)entity.InventoryAccountId);

            var InventoryAccountId = ReceivableAndPayable.Validate(Inventorylevel4.Level3_id);

            if (InventoryAccountId == false)
            {
                return new Response<CategoryDto>("Inventory account Invalid");
            }

            var Revenuelevel4 = await _unitOfWork.Level4.GetById((Guid)entity.RevenueAccountId);

            var RevenueAccountId = ReceivableAndPayable.Validate(Revenuelevel4.Level3_id);

            if (RevenueAccountId == false)
            {
                return new Response<CategoryDto>("Revenue account Invalid");
            }
            var Costlevel4 = await _unitOfWork.Level4.GetById((Guid)entity.CostAccountId);

            var CostAccountId = ReceivableAndPayable.Validate(Costlevel4.Level3_id);

            if (CostAccountId == false)
            {
                return new Response<CategoryDto>("Cost account Invalid");
            }

            if ((bool)entity.IsFixedAsset && (entity.DepreciationId == null || entity.DepreciationId == 0))
            {
                return new Response<CategoryDto>("Depreciation is Required");
            }
            if ((bool)!entity.IsFixedAsset)
            {
                category.DepreciationIdnull();
            }
            //For updating data
            _mapper.Map<CreateCategoryDto, Category>(entity, category);
            await _unitOfWork.SaveAsync();
            return new Response<CategoryDto>(_mapper.Map<CategoryDto>(category), "Updated successfully");
        }
        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<List<CategoryDto>>> GetCategoryDropDown()
        {
            var categories = await _unitOfWork.Category.GetAll();
            if (!categories.Any())
                return new Response<List<CategoryDto>>(null,"List is empty");

            return new Response<List<CategoryDto>>(_mapper.Map<List<CategoryDto>>(categories), "Returning List");
        }

        public async Task<Response<List<CategoryDto>>> GetCategoryDropDownByAsset()
        {
            var categories = await _unitOfWork.Category.GetAll(new CategorySpecs(1));
            if (!categories.Any())
                return new Response<List<CategoryDto>>(null,"List is empty");

            return new Response<List<CategoryDto>>(_mapper.Map<List<CategoryDto>>(categories), "Returning List");
        }

    }
}
