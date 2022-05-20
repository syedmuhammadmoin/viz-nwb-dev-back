using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using AutoMapper;
using Domain.Constants;
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

            var checkCNIC = _unitOfWork.Employee.Find(new EmployeeSpecs(entity.CNIC)).FirstOrDefault();

            if (checkCNIC == null)
            {
                await _unitOfWork.Employee.Add(employee);
                await _unitOfWork.SaveAsync();

                return new Response<EmployeeDto>(_mapper.Map<EmployeeDto>(checkCNIC), "Created successfully");
            }
            else
            {
                _mapper.Map<CreateEmployeeDto, Employee>(entity, checkCNIC);
                await _unitOfWork.SaveAsync();
            }

            return new Response<EmployeeDto>(_mapper.Map<EmployeeDto>(checkCNIC), "updated successfully");
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
            var getEmployee = await _unitOfWork.Employee.GetById(id, specification);
            if (getEmployee == null)
                return new Response<EmployeeDto>("Not found");

            var employee = _mapper.Map<EmployeeDto>(getEmployee);

            //getting employeelist in payrollItem
            var payrollItemList = _unitOfWork.PayrollItemEmployee
                .Find(new PayrollItemEmployeeSpecs(employee.Id, false))
                .Select(x => x.PayrollItem)
                .ToList();

            employee.PayrollItems = _mapper.Map<List<PayrollItemDto>>(payrollItemList);

            var result = MapToValue(employee);

            return new Response<EmployeeDto>(_mapper.Map<EmployeeDto>(result), "Returning value");
        }

        public async Task<Response<List<EmployeeDto>>> GetEmployeeDropDown()
        {
            var employees = await _unitOfWork.Employee.GetAll();
            if (!employees.Any())
                return new Response<List<EmployeeDto>>("List is empty");

            return new Response<List<EmployeeDto>>(_mapper.Map<List<EmployeeDto>>(employees), "Returning List");
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Response<EmployeeDto>> UpdateAsync(CreateEmployeeDto entity)
        {
            throw new NotImplementedException();
        }

        public EmployeeDto MapToValue(EmployeeDto data)
        {
            decimal totalIncrement = 0;
            decimal incrementAmount = 0;

            // if (data.IncrementId != null && data.NoOfIncrements != null)
            if (data.NoOfIncrements != null)
            {
                incrementAmount = (decimal)data.PayrollItems
                                .Where(p => p.PayrollType == PayrollType.Increment && p.IsActive == true)
                                .Sum(e => e.Value);

                totalIncrement = (incrementAmount * (int)(data.NoOfIncrements));
            }

            var basicPay = (decimal)data.PayrollItems
                                .Where(p => p.PayrollType == PayrollType.BasicPay && p.IsActive == true)
                                .Sum(e => e.Value);
            decimal totalBasicPay = (basicPay + totalIncrement);

            data.PayrollItems
               .Where(e => e.PayrollItemType == CalculationType.Percentage)
              .Select(e => e.Value = e.PayrollItemType == CalculationType.FixedAmount ? (decimal)(e.Value) :
                  (totalBasicPay * (int)(e.Value) / 100)).ToList();

            decimal totalAllowances = (decimal)data.PayrollItems
                                 .Where(p => p.PayrollType == PayrollType.Allowance || p.PayrollType == PayrollType.AssignmentAllowance && p.IsActive == true)
                                 .Sum(e => e.Value);

            decimal grossPay = totalBasicPay + totalAllowances;

            decimal totalDeductions = (decimal)data.PayrollItems
                                .Where(p => p.PayrollType == PayrollType.Deduction && p.IsActive == true)
                                .Sum(e => e.Value);

            decimal taxDeduction = (decimal)data.PayrollItems
                                .Where(p => p.PayrollType == PayrollType.TaxDeduction && p.IsActive == true)
                                .Sum(e => e.Value);

            decimal netPay = grossPay - totalDeductions - taxDeduction;

            //mapping calculated value to employeedto
            data.BasicPay = basicPay;
            data.Increment = totalIncrement;
            data.TotalAllowances = totalAllowances;
            data.TotalDeductions = totalDeductions;
            data.TaxDeduction = taxDeduction;
            data.NetPay = netPay; ;
            data.GrossPay = grossPay;
            data.TotalBasicPay = totalBasicPay;

            return data;

        }
    }
}
