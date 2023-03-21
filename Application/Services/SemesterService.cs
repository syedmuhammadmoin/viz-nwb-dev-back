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
    public class SemesterService : ISemesterService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SemesterService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginationResponse<List<SemesterDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var result = await _unitOfWork.Semester.GetAll(new SemesterSpecs(filter, false));
            if (result.Count() == 0)
                return new PaginationResponse<List<SemesterDto>>(_mapper.Map<List<SemesterDto>>(result), "List is empty");

            var totalRecords = await _unitOfWork.Semester.TotalRecord(new SemesterSpecs(filter, true));
            return new PaginationResponse<List<SemesterDto>>(_mapper.Map<List<SemesterDto>>(result), filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<SemesterDto>> GetByIdAsync(int id)
        {
            var result = await _unitOfWork.Semester.GetById(id);
            if (result == null)
                return new Response<SemesterDto>("Not found");

            return new Response<SemesterDto>(_mapper.Map<SemesterDto>(result), "Returning value");
        }

        public async Task<Response<List<SemesterDto>>> GetDropDown()
        {
            var result = await _unitOfWork.Semester.GetAll();
            if (!result.Any())
                return new Response<List<SemesterDto>>(null, "List is empty");

            return new Response<List<SemesterDto>>(_mapper.Map<List<SemesterDto>>(result), "Returning List");
        }

        public async Task<Response<SemesterDto>> CreateAsync(SemesterDto entity)
        {
            var result = await _unitOfWork.Semester.Add(_mapper.Map<Semester>(entity));
            await _unitOfWork.SaveAsync();
            return new Response<SemesterDto>(_mapper.Map<SemesterDto>(result), "Created successfully");
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<SemesterDto>> UpdateAsync(SemesterDto entity)
        {
            var result = await _unitOfWork.Semester.GetById((int)entity.Id);
            if (result == null)
                return new Response<SemesterDto>("Not found");

            //For updating data
            _mapper.Map(entity, result);
            await _unitOfWork.SaveAsync();
            return new Response<SemesterDto>(_mapper.Map<SemesterDto>(result), "Updated successfully");
        }
    }
}
