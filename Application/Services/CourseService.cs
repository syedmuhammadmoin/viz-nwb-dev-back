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
    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CourseService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginationResponse<List<CourseDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var result = await _unitOfWork.Course.GetAll(new CourseSpecs(filter, false));
            if (result.Count() == 0)
                return new PaginationResponse<List<CourseDto>>(_mapper.Map<List<CourseDto>>(result), "List is empty");

            var totalRecords = await _unitOfWork.Course.TotalRecord(new CourseSpecs(filter, true));
            return new PaginationResponse<List<CourseDto>>(_mapper.Map<List<CourseDto>>(result), filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<CourseDto>> GetByIdAsync(int id)
        {
            var result = await _unitOfWork.Course.GetById(id);
            if (result == null)
                return new Response<CourseDto>("Not found");

            return new Response<CourseDto>(_mapper.Map<CourseDto>(result), "Returning value");
        }

        public async Task<Response<List<CourseDto>>> GetDropDown()
        {
            var result = await _unitOfWork.Course.GetAll();
            if (!result.Any())
                return new Response<List<CourseDto>>(null, "List is empty");

            return new Response<List<CourseDto>>(_mapper.Map<List<CourseDto>>(result), "Returning List");
        }

        public async Task<Response<CourseDto>> CreateAsync(CourseDto entity)
        {
            var result = await _unitOfWork.Course.Add(_mapper.Map<Course>(entity));
            await _unitOfWork.SaveAsync();
            return new Response<CourseDto>(_mapper.Map<CourseDto>(result), "Created successfully");
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<CourseDto>> UpdateAsync(CourseDto entity)
        {
            var result = await _unitOfWork.Course.GetById((int)entity.Id);
            if (result == null)
                return new Response<CourseDto>("Not found");

            //For updating data
            _mapper.Map(entity, result);
            await _unitOfWork.SaveAsync();
            return new Response<CourseDto>(_mapper.Map<CourseDto>(result), "Updated successfully");
        }
    }
}
