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
            var result = await _unitOfWork.Category.Add(category);
            await _unitOfWork.SaveAsync();

            return new Response<CategoryDto>(_mapper.Map<CategoryDto>(result), "Created successfully");
        }

        public async Task<PaginationResponse<List<CategoryDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var specification = new CategorySpecs(filter);
            var category = await _unitOfWork.Category.GetAll(specification);

            if (category.Count() == 0)
                return new PaginationResponse<List<CategoryDto>>(_mapper.Map<List<CategoryDto>>(category), "List is empty");

            var totalRecords = await _unitOfWork.Category.TotalRecord(specification);

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
                return new Response<List<CategoryDto>>("List is empty");

            return new Response<List<CategoryDto>>(_mapper.Map<List<CategoryDto>>(categories), "Returning List");
        }
    }
}
