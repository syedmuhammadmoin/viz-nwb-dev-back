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
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<EmployeeDto>> CreateAsync(CreateEmployeeDto entity)
        {
            var employee = _mapper.Map<Employee>(entity);
            var result = await _unitOfWork.Employee.Add(employee);
            await _unitOfWork.SaveAsync();

            return new Response<EmployeeDto>(_mapper.Map<EmployeeDto>(result), "Created successfully");
        }

        public async Task<PaginationResponse<List<EmployeeDto>>> GetAllAsync(PaginationFilter filter)
        {
            var specification = new EmployeeSpecs(filter);
            var employees = await _unitOfWork.Employee.GetAll(specification);

            if (employees.Count() == 0)
                return new PaginationResponse<List<EmployeeDto>>("List is empty");

            var totalRecords = await _unitOfWork.Employee.TotalRecord();

            return new PaginationResponse<List<EmployeeDto>>(_mapper.Map<List<EmployeeDto>>(employees), filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<EmployeeDto>> GetByIdAsync(int id)
        {
            var specification = new EmployeeSpecs();
            var employee = await _unitOfWork.Employee.GetById(id, specification); 
            if (employee == null)
                return new Response<EmployeeDto>("Not found");

            return new Response<EmployeeDto>(_mapper.Map<EmployeeDto>(employee), "Returning value");
        }

        public async Task<Response<List<EmployeeDto>>> GetEmployeeDropDown()
        {
            var employees = await _unitOfWork.Employee.GetAll();
            if (!employees.Any())
                return new Response<List<EmployeeDto>>("List is empty");

            return new Response<List<EmployeeDto>>(_mapper.Map<List<EmployeeDto>>(employees), "Returning List");
        }

        public async Task<Response<EmployeeDto>> UpdateAsync(CreateEmployeeDto entity)
        {
            var employee = await _unitOfWork.Employee.GetById((int)entity.Id);

            if (employee == null)
                return new Response<EmployeeDto>("Not found");

            //For updating data
            _mapper.Map<CreateEmployeeDto, Employee>(entity, employee);
            await _unitOfWork.SaveAsync();
            return new Response<EmployeeDto>(_mapper.Map<EmployeeDto>(employee), "Updated successfully");
        }
        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

    }
}
