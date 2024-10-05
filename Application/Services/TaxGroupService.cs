using Application.Contracts.DTOs;
using Application.Contracts.DTOs.TaxGroup;
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
    public class TaxGroupService : ITaxGroupService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TaxGroupService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<TaxGroupDto>> CreateAsync(CreateTaxGroupDto entity)
        {
            var result = await _unitOfWork.TaxGroup.Add(_mapper.Map<TaxGroup>(entity));
            await _unitOfWork.SaveAsync();
            return new Response<TaxGroupDto>(_mapper.Map<TaxGroupDto>(result),"Created Successfully.");
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<bool>> DeleteTaxGroups(List<int> ids)
        {
            if (ids.Count() < 0 || ids == null)
            {
                return new Response<bool>("List Cannot be Empty.");
            }
            else
            {
                foreach (var coa in ids)
                {
                    var Jv = await _unitOfWork.TaxGroup.GetById(coa);
                    if (Jv == null)
                        return new Response<bool>("Tax Group Not Found.");
                    Jv.IsDelete = true;
                    await _unitOfWork.SaveAsync();
                }
                return new Response<bool>(true, "Deleted Successfully.");
            }
        }

        public async Task<PaginationResponse<List<TaxGroupDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var taxesGrps = await _unitOfWork.TaxGroup.GetAll(new TaxGroupSpecs(filter, false));

            if (taxesGrps.Count() == 0)
                return new PaginationResponse<List<TaxGroupDto>>(_mapper.Map<List<TaxGroupDto>>(taxesGrps), "List is empty");

            var totalRecords = await _unitOfWork.TaxGroup.TotalRecord(new TaxGroupSpecs(filter, true));

            return new PaginationResponse<List<TaxGroupDto>>(_mapper.Map<List<TaxGroupDto>>(taxesGrps), filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<TaxGroupDto>> GetByIdAsync(int id)
        {
           var taxGrp = await _unitOfWork.TaxGroup.GetById(id);
            if (taxGrp == null)
                return new Response<TaxGroupDto>("Tax Group Not Found.");
            return new Response<TaxGroupDto>(_mapper.Map<TaxGroupDto>(taxGrp), "Returning Tax");
        }

        public async Task<Response<List<TaxGroupDto>>> GetTaxDropdown()
        {
            var result = await _unitOfWork.TaxGroup.GetAll();
            if (!result.Any())
                return new Response<List<TaxGroupDto>>("List is Empty.");
            return new Response<List<TaxGroupDto>>(_mapper.Map<List<TaxGroupDto>>(result),"List is Empty.");
        }

        public async Task<Response<TaxGroupDto>> UpdateAsync(CreateTaxGroupDto entity)
        {
            var taxGrp = await _unitOfWork.TaxGroup.GetById(entity.Id);
            if (taxGrp == null)
                return new Response<TaxGroupDto>("Tax Group Not Found.");
            _mapper.Map(entity,taxGrp);
            await _unitOfWork.SaveAsync();
            return new Response<TaxGroupDto>(_mapper.Map<TaxGroupDto>(taxGrp), "Updated Successfulyy.");
        }
    }
}
