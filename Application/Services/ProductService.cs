using Application.Contracts.DTOs.Products;
using Application.Contracts.Filters;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using AutoMapper;
using Domain.Interfaces;
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

        public Task<Response<ProductDto>> CreateAsync(CreateProductDto entity)
        {
            throw new NotImplementedException();
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PaginationResponse<List<ProductDto>>> GetAllAsync(PaginationFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task<Response<ProductDto>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Response<ProductDto>> UpdateAsync(CreateProductDto entity)
        {
            throw new NotImplementedException();
        }
    }
}
