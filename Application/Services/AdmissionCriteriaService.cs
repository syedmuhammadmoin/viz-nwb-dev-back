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
    public class AdmissionCriteriaService : IAdmissionCriteriaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AdmissionCriteriaService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginationResponse<List<AdmissionCriteriaDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var result = await _unitOfWork.AdmissionCriteria.GetAll(new AdmissionCriteriaSpecs(filter, false));
            if (result.Count() == 0)
                return new PaginationResponse<List<AdmissionCriteriaDto>>(_mapper.Map<List<AdmissionCriteriaDto>>(result), "List is empty");

            var totalRecords = await _unitOfWork.AdmissionCriteria.TotalRecord(new AdmissionCriteriaSpecs(filter, true));
            return new PaginationResponse<List<AdmissionCriteriaDto>>(_mapper.Map<List<AdmissionCriteriaDto>>(result), filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<AdmissionCriteriaDto>> GetByIdAsync(int id)
        {
            var result = await _unitOfWork.AdmissionCriteria.GetById(id, new AdmissionCriteriaSpecs());
            if (result == null)
                return new Response<AdmissionCriteriaDto>("Not found");

            return new Response<AdmissionCriteriaDto>(_mapper.Map<AdmissionCriteriaDto>(result), "Returning value");
        }

        public async Task<Response<AdmissionCriteriaDto>> CreateAsync(CreateAdmissionCriteriaDto entity)
        {
            var result = await _unitOfWork.AdmissionCriteria.Add(_mapper.Map<AdmissionCriteria>(entity));
            await _unitOfWork.SaveAsync();
            return new Response<AdmissionCriteriaDto>(_mapper.Map<AdmissionCriteriaDto>(result), "Created successfully");
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<AdmissionCriteriaDto>> UpdateAsync(CreateAdmissionCriteriaDto entity)
        {
            var result = await _unitOfWork.AdmissionCriteria.GetById((int)entity.Id);
            if (result == null)
                return new Response<AdmissionCriteriaDto>("Not found");

            //For updating data
            _mapper.Map(entity, result);
            await _unitOfWork.SaveAsync();
            return new Response<AdmissionCriteriaDto>(_mapper.Map<AdmissionCriteriaDto>(result), "Updated successfully");
        }

    }
}
