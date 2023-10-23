using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Specifications;

namespace Application.Services
{
    public class DomicileService : IDomicileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DomicileService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginationResponse<List<DomicileDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var result = await _unitOfWork.Domicile.GetAll(new DomicileSpecs(filter, false));
            if (result.Count() == 0)
                return new PaginationResponse<List<DomicileDto>>(_mapper.Map<List<DomicileDto>>(result), "List is empty");

            var totalRecords = await _unitOfWork.Domicile.TotalRecord(new DomicileSpecs(filter, true));
            return new PaginationResponse<List<DomicileDto>>(_mapper.Map<List<DomicileDto>>(result), filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<DomicileDto>> GetByIdAsync(int id)
        {
            var result = await _unitOfWork.Domicile.GetById(id, new DomicileSpecs());
            if (result == null)
                return new Response<DomicileDto>("Not found");

            return new Response<DomicileDto>(_mapper.Map<DomicileDto>(result), "Returning value");
        }

        public async Task<Response<List<DomicileDto>>> GetDropDown()
        {
            var result = await _unitOfWork.Domicile.GetAll();
            if (!result.Any())
                return new Response<List<DomicileDto>>(null, "List is empty");

            return new Response<List<DomicileDto>>(_mapper.Map<List<DomicileDto>>(result), "Returning List");
        }

        public async Task<Response<List<DomicileDto>>> GetByDistrict(int districtId)
        {
            var result = await _unitOfWork.Domicile.GetAll(new DomicileSpecs(districtId));
            if (!result.Any())
                return new Response<List<DomicileDto>>(null, "List is empty");

            return new Response<List<DomicileDto>>(_mapper.Map<List<DomicileDto>>(result), "Returning List");
        }

        public async Task<Response<DomicileDto>> CreateAsync(CreateDomicileDto entity)
        {
            var result = await _unitOfWork.Domicile.Add(_mapper.Map<Domicile>(entity));
            await _unitOfWork.SaveAsync();
            return new Response<DomicileDto>(_mapper.Map<DomicileDto>(result), "Created successfully");
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<DomicileDto>> UpdateAsync(CreateDomicileDto entity)
        {
            var result = await _unitOfWork.Domicile.GetById((int)entity.Id);
            if (result == null)
                return new Response<DomicileDto>("Not found");

            //For updating data
            _mapper.Map(entity, result);
            await _unitOfWork.SaveAsync();
            return new Response<DomicileDto>(_mapper.Map<DomicileDto>(result), "Updated successfully");
        }

    }
}
