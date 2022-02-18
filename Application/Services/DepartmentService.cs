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
        public async Task<Response<DeptDto>> CreateAsync(CreateDeptDto entity)
        {
            var dept = _mapper.Map<Department>(entity);
            var result = await _unitOfWork.Department.Add(dept);
            await _unitOfWork.SaveAsync();

            return new Response<DeptDto>(_mapper.Map<DeptDto>(result), "Created successfully");
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PaginationResponse<List<DeptDto>>> GetAllAsync(PaginationFilter filter)
        {
            var specification = new DepartmentSpecs(filter);
            var dept = await _unitOfWork.Department.GetAll(specification);

            if (dept.Count() == 0)
                return new PaginationResponse<List<DeptDto>>("List is empty");

            var totalRecords = await _unitOfWork.Department.TotalRecord();

            return new PaginationResponse<List<DeptDto>>(_mapper.Map<List<DeptDto>>(dept), filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<DeptDto>> GetByIdAsync(int id)
        {
            var dept = await _unitOfWork.Department.GetById(id);
            if (dept == null)
                return new Response<DeptDto>("Not found");

            return new Response<DeptDto>(_mapper.Map<DeptDto>(dept), "Returning value");
        }

        public async Task<Response<DeptDto>> UpdateAsync(CreateDeptDto entity)
        {
            var dept = await _unitOfWork.Department.GetById((int)entity.Id);

            if (dept == null)
                return new Response<DeptDto>("Not found");

            //For updating data
            _mapper.Map<CreateDeptDto, Department>(entity, dept);
            await _unitOfWork.SaveAsync();
            return new Response<DeptDto>(_mapper.Map<DeptDto>(dept), "Updated successfully");
        }
    }
}
