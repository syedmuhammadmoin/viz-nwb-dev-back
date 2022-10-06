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

        public async Task<Response<DepartmentDto>> CreateAsync(CreateDepartmentDto[] entity)
        {
            List<Department> departmentList = new List<Department>();
            List<string> errors = new List<string>();

            foreach (var item in entity)
            {
                var getDepartment = await _unitOfWork.Department.GetById((int)item.Id);

                //for Checking if campus is Active
                var getCampus = await _unitOfWork.Campus.GetById(item.CampusId);

                if (getCampus.IsActive == true)
                {
                    if (getDepartment != null)
                    {
                        _mapper.Map<CreateDepartmentDto, Department>(item, getDepartment);
                    }
                    else
                    {
                        departmentList.Add(_mapper.Map<Department>(item));
                    }
                }
                else
                {
                   //returning departments with inactive Campuses
                    errors.Add($"Campus for {item.Name} is not Active");
                }
            }

            await _unitOfWork.SaveAsync();

            if (departmentList.Any())
            {
                await _unitOfWork.Department.AddRange(departmentList);
                await _unitOfWork.SaveAsync();
            }

            var message = "Records populated successfully";

            if (errors.Count > 0)
            {
                message = "Records populated successfully with errors";
            }
            return new Response<DepartmentDto>(null, message, errors.ToArray());
        }

        public async Task<PaginationResponse<List<DepartmentDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var departments = await _unitOfWork.Department.GetAll(new DepartmentSpecs(filter, false));

            if (departments.Count() == 0)
                return new PaginationResponse<List<DepartmentDto>>(_mapper.Map<List<DepartmentDto>>(departments), "List is empty");

            var totalRecords = await _unitOfWork.Department.TotalRecord(new DepartmentSpecs(filter, true));

            return new PaginationResponse<List<DepartmentDto>>(_mapper.Map<List<DepartmentDto>>(departments), filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<DepartmentDto>> GetByIdAsync(int id)
        {
            var department = await _unitOfWork.Department.GetById(id, new DepartmentSpecs());
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

        public Task<Response<DepartmentDto>> UpdateAsync(CreateDepartmentDto[] entity)
        {
            throw new NotImplementedException();
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<List<DepartmentDto>>> GetDepartmentByCampusDropDown(int campusId)
        {
            var warehouses = await _unitOfWork.Department.GetAll(new DepartmentSpecs(campusId)) ;
            if (!warehouses.Any())
                return new Response<List<DepartmentDto>>("List is empty");

            return new Response<List<DepartmentDto>>(_mapper.Map<List<DepartmentDto>>(warehouses), "Returning List");
        }
    }
}
