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
    public class SubjectService : ISubjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SubjectService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginationResponse<List<SubjectDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var result = await _unitOfWork.Subject.GetAll(new SubjectSpecs(filter, false));
            if (result.Count() == 0)
                return new PaginationResponse<List<SubjectDto>>(_mapper.Map<List<SubjectDto>>(result), "List is empty");

            var totalRecords = await _unitOfWork.Subject.TotalRecord(new SubjectSpecs(filter, true));
            return new PaginationResponse<List<SubjectDto>>(_mapper.Map<List<SubjectDto>>(result), filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<SubjectDto>> GetByIdAsync(int id)
        {
            var result = await _unitOfWork.Subject.GetById(id, new SubjectSpecs());
            if (result == null)
                return new Response<SubjectDto>("Not found");

            return new Response<SubjectDto>(_mapper.Map<SubjectDto>(result), "Returning value");
        }

        public async Task<Response<List<SubjectDto>>> GetDropDown()
        {
            var result = await _unitOfWork.Subject.GetAll();
            if (!result.Any())
                return new Response<List<SubjectDto>>(null, "List is empty");

            return new Response<List<SubjectDto>>(_mapper.Map<List<SubjectDto>>(result), "Returning List");
        }

        public async Task<Response<SubjectDto>> CreateAsync(CreateSubjectDto entity)
        {
            var result = await _unitOfWork.Subject.Add(_mapper.Map<Subject>(entity));
            await _unitOfWork.SaveAsync();
            return new Response<SubjectDto>(_mapper.Map<SubjectDto>(result), "Created successfully");
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<SubjectDto>> UpdateAsync(CreateSubjectDto entity)
        {
            var result = await _unitOfWork.Subject.GetById((int)entity.Id);
            if (result == null)
                return new Response<SubjectDto>("Not found");

            //For updating data
            _mapper.Map(entity, result);
            await _unitOfWork.SaveAsync();
            return new Response<SubjectDto>(_mapper.Map<SubjectDto>(result), "Updated successfully");
        }
    }
}
