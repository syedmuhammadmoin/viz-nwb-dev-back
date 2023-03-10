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
    public class FacultyService : IFacultyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FacultyService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginationResponse<List<FacultyDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var result = await _unitOfWork.Faculty.GetAll(new FacultySpecs(filter, false));
            if (result.Count() == 0)
                return new PaginationResponse<List<FacultyDto>>(_mapper.Map<List<FacultyDto>>(result), "List is empty");

            var totalRecords = await _unitOfWork.Faculty.TotalRecord(new FacultySpecs(filter, true));
            return new PaginationResponse<List<FacultyDto>>(_mapper.Map<List<FacultyDto>>(result), filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<FacultyDto>> GetByIdAsync(int id)
        {
            var result = await _unitOfWork.Faculty.GetById(id);
            if (result == null)
                return new Response<FacultyDto>("Not found");

            return new Response<FacultyDto>(_mapper.Map<FacultyDto>(result), "Returning value");
        }

        public async Task<Response<List<FacultyDto>>> GetDropDown()
        {
            var result = await _unitOfWork.Faculty.GetAll();
            if (!result.Any())
                return new Response<List<FacultyDto>>(null, "List is empty");

            return new Response<List<FacultyDto>>(_mapper.Map<List<FacultyDto>>(result), "Returning List");
        }

        public async Task<Response<FacultyDto>> CreateAsync(CreateFacultyDto entity)
        {
            var result = await _unitOfWork.Faculty.Add(_mapper.Map<Faculty>(entity));
            await _unitOfWork.SaveAsync();
            return new Response<FacultyDto>(_mapper.Map<FacultyDto>(result), "Created successfully");
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<FacultyDto>> UpdateAsync(CreateFacultyDto entity)
        {
            var result = await _unitOfWork.Faculty.GetById((int)entity.Id);
            if (result == null)
                return new Response<FacultyDto>("Not found");

            //For updating data
            _mapper.Map(entity, result);
            await _unitOfWork.SaveAsync();
            return new Response<FacultyDto>(_mapper.Map<FacultyDto>(result), "Updated successfully");
        }

    }
}
