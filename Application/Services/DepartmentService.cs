using Application.Contracts.DTOs;
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
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepartmentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<DepartmentDto>> CreateAsync(DepartmentDto entity)
        {
            var department = _mapper.Map<Department>(entity);

            var getDepartment = await _unitOfWork.Department.GetById((int)entity.Id);

            if (getDepartment != null)
            {
                _mapper.Map<DepartmentDto, Department>(entity, getDepartment);
                await _unitOfWork.SaveAsync();

                return new Response<DepartmentDto>(_mapper.Map<DepartmentDto>(getDepartment), "Updated successfully");
            }
            else
            {
                await _unitOfWork.Department.Add(department);
                await _unitOfWork.SaveAsync();
            }

            return new Response<DepartmentDto>(_mapper.Map<DepartmentDto>(getDepartment), "Created successfully");
        }

        public async Task<PaginationResponse<List<DepartmentDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var specification = new DepartmentSpecs(filter);
            var departments = await _unitOfWork.Department.GetAll(specification);

            if (departments.Count() == 0)
                return new PaginationResponse<List<DepartmentDto>>(_mapper.Map<List<DepartmentDto>>(departments), "List is empty");

            var totalRecords = await _unitOfWork.Department.TotalRecord(specification);

            return new PaginationResponse<List<DepartmentDto>>(_mapper.Map<List<DepartmentDto>>(departments), filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<DepartmentDto>> GetByIdAsync(int id)
        {
            var department = await _unitOfWork.Department.GetById(id);
            if (department == null)
                return new Response<DepartmentDto>("Not found");

            return new Response<DepartmentDto>(_mapper.Map<DepartmentDto>(department), "Returning value");
        }

        public async Task<Response<List<DepartmentDto>>> GetDepartmentDropDown()
        {
            var departments = await _unitOfWork.Department.GetAll();
            if (!departments.Any())
                return new Response<List<DepartmentDto>>("List is empty");

            return new Response<List<DepartmentDto>>(_mapper.Map<List<DepartmentDto>>(departments), "Returning List");
        }

        public Task<Response<DepartmentDto>> UpdateAsync(DepartmentDto entity)
        {
            throw new NotImplementedException();
        }
        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

    }
}
