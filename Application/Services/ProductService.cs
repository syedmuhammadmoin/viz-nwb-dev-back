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
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<ProductDto>> CreateAsync(CreateProductDto entity)
        {
            var product = _mapper.Map<Product>(entity);
            var result = await _unitOfWork.Product.Add(product);
            await _unitOfWork.SaveAsync();

            return new Response<ProductDto>(_mapper.Map<ProductDto>(result), "Created successfully");
        }

        public async Task<PaginationResponse<List<ProductDto>>> GetAllAsync(PaginationFilter filter)
        {
            var specification = new ProductSpecs(filter);
            var product = await _unitOfWork.Product.GetAll(specification);

            if (product.Count() == 0)
                return new PaginationResponse<List<ProductDto>>(_mapper.Map<List<ProductDto>>(product), "List is empty");

            var totalRecords = await _unitOfWork.Product.TotalRecord();

            return new PaginationResponse<List<ProductDto>>(_mapper.Map<List<ProductDto>>(product), filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<ProductDto>> GetByIdAsync(int id)
        {
            var specification = new ProductSpecs();
            var product = await _unitOfWork.Product.GetById(id, specification);
            if (product == null)
                return new Response<ProductDto>("Not found");

            return new Response<ProductDto>(_mapper.Map<ProductDto>(product), "Returning value");
        }

        public async Task<Response<ProductDto>> UpdateAsync(CreateProductDto entity)
        {
            var product = await _unitOfWork.Product.GetById((int)entity.Id);

            if (product == null)
                return new Response<ProductDto>("Not found");

            //For updating data
            _mapper.Map<CreateProductDto, Product>(entity, product);
            await _unitOfWork.SaveAsync();
            return new Response<ProductDto>(_mapper.Map<ProductDto>(product), "Updated successfully");
        }
        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<List<ProductDto>>> GetProductDropDown()
        {
            var products = await _unitOfWork.Product.GetAll();
            if (!products.Any())
                return new Response<List<ProductDto>>("List is empty");

            return new Response<List<ProductDto>>(_mapper.Map<List<ProductDto>>(products), "Returning List");
        }
    }
}
