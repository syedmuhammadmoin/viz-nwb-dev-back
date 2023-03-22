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
    public class QualificationService : IQualificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public QualificationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginationResponse<List<QualificationDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var result = await _unitOfWork.Qualification.GetAll(new QualificationSpecs(filter, false));
            if (result.Count() == 0)
                return new PaginationResponse<List<QualificationDto>>(_mapper.Map<List<QualificationDto>>(result), "List is empty");

            var totalRecords = await _unitOfWork.Qualification.TotalRecord(new QualificationSpecs(filter, true));
            return new PaginationResponse<List<QualificationDto>>(_mapper.Map<List<QualificationDto>>(result), filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<QualificationDto>> GetByIdAsync(int id)
        {
            var result = await _unitOfWork.Qualification.GetById(id);
            if (result == null)
                return new Response<QualificationDto>("Not found");

            return new Response<QualificationDto>(_mapper.Map<QualificationDto>(result), "Returning value");
        }

        public async Task<Response<List<QualificationDto>>> GetDropDown()
        {
            var result = await _unitOfWork.Qualification.GetAll();
            if (!result.Any())
                return new Response<List<QualificationDto>>(null, "List is empty");

            return new Response<List<QualificationDto>>(_mapper.Map<List<QualificationDto>>(result), "Returning List");
        }

        public async Task<Response<QualificationDto>> CreateAsync(QualificationDto entity)
        {
            var result = await _unitOfWork.Qualification.Add(_mapper.Map<Qualification>(entity));
            await _unitOfWork.SaveAsync();
            return new Response<QualificationDto>(_mapper.Map<QualificationDto>(result), "Created successfully");
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<QualificationDto>> UpdateAsync(QualificationDto entity)
        {
            var result = await _unitOfWork.Qualification.GetById((int)entity.Id);
            if (result == null)
                return new Response<QualificationDto>("Not found");

            //For updating data
            _mapper.Map(entity, result);
            await _unitOfWork.SaveAsync();
            return new Response<QualificationDto>(_mapper.Map<QualificationDto>(result), "Updated successfully");
        }
    }
}
