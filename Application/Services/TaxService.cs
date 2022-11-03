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
            var taxes = await _unitOfWork.Taxes.GetAll(new TaxesSpecs(filter, false));

            if (taxes.Count() == 0)
                return new PaginationResponse<List<TaxDto>>(_mapper.Map<List<TaxDto>>(taxes), "List is empty");

            var totalRecords = await _unitOfWork.Taxes.TotalRecord(new TaxesSpecs(filter, true));

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

            var taxLevel4 = await _unitOfWork.Level4.GetById((Guid)entity.AccountId);

            var AccountId = ReceivableAndPayable.Validate(taxLevel4.Level3_id);

            if(AccountId == false)
            {
                return new  Response<TaxDto>("Account Invalid");
            }
            
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
