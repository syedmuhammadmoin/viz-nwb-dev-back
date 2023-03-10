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
    public class AcademicDepartmentService : IAcademicDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AcademicDepartmentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginationResponse<List<AcademicDepartmentDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var result = await _unitOfWork.AcademicDepartment.GetAll(new AcademicDepartmentSpecs(filter, false));
            if (result.Count() == 0)
                return new PaginationResponse<List<AcademicDepartmentDto>>(_mapper.Map<List<AcademicDepartmentDto>>(result), "List is empty");

            var totalRecords = await _unitOfWork.AcademicDepartment.TotalRecord(new AcademicDepartmentSpecs(filter, true));
            return new PaginationResponse<List<AcademicDepartmentDto>>(_mapper.Map<List<AcademicDepartmentDto>>(result), filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<AcademicDepartmentDto>> GetByIdAsync(int id)
        {
            var result = await _unitOfWork.AcademicDepartment.GetById(id, new AcademicDepartmentSpecs());
            if (result == null)
                return new Response<AcademicDepartmentDto>("Not found");

            return new Response<AcademicDepartmentDto>(_mapper.Map<AcademicDepartmentDto>(result), "Returning value");
        }

        public async Task<Response<List<AcademicDepartmentDto>>> GetDropDown()
        {
            var result = await _unitOfWork.AcademicDepartment.GetAll();
            if (!result.Any())
                return new Response<List<AcademicDepartmentDto>>(null, "List is empty");

            return new Response<List<AcademicDepartmentDto>>(_mapper.Map<List<AcademicDepartmentDto>>(result), "Returning List");
        }

        public async Task<Response<AcademicDepartmentDto>> CreateAsync(CreateAcademicDepartmentDto entity)
        {
            var result = await _unitOfWork.AcademicDepartment.Add(_mapper.Map<AcademicDepartment>(entity));
            await _unitOfWork.SaveAsync();
            return new Response<AcademicDepartmentDto>(_mapper.Map<AcademicDepartmentDto>(result), "Created successfully");
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<AcademicDepartmentDto>> UpdateAsync(CreateAcademicDepartmentDto entity)
        {
            var result = await _unitOfWork.AcademicDepartment.GetById((int)entity.Id);
            if (result == null)
                return new Response<AcademicDepartmentDto>("Not found");

            //For updating data
            _mapper.Map(entity, result);
            await _unitOfWork.SaveAsync();
            return new Response<AcademicDepartmentDto>(_mapper.Map<AcademicDepartmentDto>(result), "Updated successfully");
        }
    }
}
