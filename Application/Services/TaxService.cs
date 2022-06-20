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
    public class TaxService : ITaxService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TaxService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginationResponse<List<TaxDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var specification = new TaxesSpecs(filter);
            var taxes = await _unitOfWork.Taxes.GetAll(specification);

            if (taxes.Count() == 0)
                return new PaginationResponse<List<TaxDto>>(_mapper.Map<List<TaxDto>>(taxes), "List is empty");

            var totalRecords = await _unitOfWork.Taxes.TotalRecord(specification);

            return new PaginationResponse<List<TaxDto>>(_mapper.Map<List<TaxDto>>(taxes), filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<TaxDto>> GetByIdAsync(int id)
        {

            var specification = new TaxesSpecs();
            var tax = await _unitOfWork.Taxes.GetById(id, specification);
            if (tax == null)
                return new Response<TaxDto>("Not found");

            return new Response<TaxDto>(_mapper.Map<TaxDto>(tax), "Returning value");
        }

        public async Task<Response<TaxDto>> UpdateAsync(UpdateTaxDto entity)

        {
            var tax = await _unitOfWork.Taxes.GetById((int)entity.Id);

            if (tax == null)
                return new Response<TaxDto>("Not found");

            //For updating data
            _mapper.Map<UpdateTaxDto, Taxes>(entity, tax);
            await _unitOfWork.SaveAsync();
            return new Response<TaxDto>(_mapper.Map<TaxDto>(tax), "Updated successfully");
        }
        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Response<TaxDto>> CreateAsync(UpdateTaxDto entity)
        {
            throw new NotImplementedException();
        }
    }
}
