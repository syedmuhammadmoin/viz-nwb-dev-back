using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Helper;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Specifications;
using Infrastructure.Uow;
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

        public async Task<Response<TaxDto>> UpdateAsync(CreateTaxDto entity)
        {
            var specs = new TaxesSpecs();
            var tax = await _unitOfWork.Taxes.GetById((int)entity.Id,specs);

            if (tax == null)
                return new Response<TaxDto>("Not found");

            var taxLevel4 = await _unitOfWork.Level4.GetById(entity.AccountId);

            //SBBU-Code
            //var AccountId = ReceivableAndPayable.Validate(taxLevel4.Level3_id);

            ////Validation For Receivable and Payable
            //if(AccountId == false)
            //{
            //    return new  Response<TaxDto>("Account Invalid");
            //}

            //For updating data
            _unitOfWork.CreateTransaction();
            _mapper.Map<CreateTaxDto, Taxes>(entity, tax);
            await _unitOfWork.SaveAsync();
            _unitOfWork.Commit();
            return new Response<TaxDto>(_mapper.Map<TaxDto>(tax), "Updated successfully");
        }
        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<TaxDto>> CreateAsync(CreateTaxDto entity)
        {
            var tax = _mapper.Map<Taxes>(entity);
            await _unitOfWork.Taxes.Add(tax);
            await _unitOfWork.SaveAsync();
            return new Response<TaxDto>(_mapper.Map<TaxDto>(tax), "Create successfully");
        }

        public async Task<Response<bool>> DeleteTaxes(List<int> ids)
        {
            if (ids.Count() < 0 || ids == null)
            {
                return new Response<bool>("List Cannot be Empty.");
            }
            else
            {
                foreach (var coa in ids)
                {
                    var Jv = await _unitOfWork.Taxes.GetById(coa);
                    if (Jv == null)
                        return new Response<bool>("Tax Not Found.");
                    Jv.IsDelete = true;
                    await _unitOfWork.SaveAsync();
                }
                return new Response<bool>(true, "Deleted Successfully.");
            }
        }

        public async Task<Response<List<TaxDto>>> GetTaxesWithIds(List<int> ids)
        {
            if (ids == null || !ids.Any())
                return new Response<List<TaxDto>>("No IDs provided");
            var taxes = await _unitOfWork.Taxes.GetAll();
            if (taxes == null)
                return new Response<List<TaxDto>>("List is Empty");
            var selectedTaxes = taxes.Where(x => ids.Contains(x.Id)).ToList();
            return new Response<List<TaxDto>>(_mapper.Map<List<TaxDto>>(selectedTaxes), "Returning List");
        }

        public async Task<Response<bool>> InActiveTax(int id,bool status)
        {
            var result = await _unitOfWork.Taxes.GetById(id);
            if (result == null)
                return new Response<bool>("Could not find record.");
            result.SetActiveStatus(status);
           await _unitOfWork.SaveAsync();
            return new Response<bool>(status, "Status Updated");
        }
    }
}
