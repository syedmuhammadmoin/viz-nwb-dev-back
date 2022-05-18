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
                .Find(new PayrollItemEmployeeSpecs(employee.Id,false))
                .Select(x => x.PayrollItem)
                .ToList();

            employee.PayrollItems = _mapper.Map<List<PayrollItemDto>>(payrollItemList);

            var result = MapToValue(employee);

            return new Response<EmployeeDto>(_mapper.Map<EmployeeDto>(employee), "Returning value");
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

        public Task<Response<EmployeeDto>> MapToValue(EmployeeDto data)
        {
            decimal totalIncrement = 0;
            decimal incrementAmount = 0;

            if (data.IncrementId != null && data.NoOFIncrements != null)
            {
                incrementAmount = (decimal)data.Increment.Amount;
                totalIncrement = (decimal)(data.Increment.Amount * (int)(data.NoOFIncrements));
            }

            decimal totalBasicPay = (decimal)(data.BasicPay.Amount + totalIncrement);

            var payrollItemList = _unitOfWork.PayrollItemEmployee
                .Find(new PayrollItemEmployeeSpecs(data.Id, false))
                .Select(x => x.PayrollItem)
                .ToList();

            //Getting emp Lines
            //var empLines = data.PayrollItems
            //    .Select(e => new EmployeeLinesDTO
            //    {
            //        Id = e.Id,
            //        PayrollItemId = e.PayrollItemId,
            //        PayrollItem = e.PayrollItem.Name,
            //        PayrollType = e.PayrollItem.PayrollType,
            //        AccountId = e.PayrollItem.AccountId,
            //        Account = e.PayrollItem.Account.Name,
            //        isActive = e.PayrollItem.isActive,
            //        Amount = e.PayrollItem.PayrollItemType == PayrollItemType.FixedAmount ? (decimal)(e.PayrollItem.Amount) :
            //        (totalBasicPay * (int)(e.PayrollItem.Percentage) / 100)
            //    }).ToList();

            decimal totalAllowances = empLines
                                .Where(p => p.PayrollType == PayrollType.Allowance || p.PayrollType == PayrollType.AssignmentAllowance)
                                .Sum(e => e.Amount);

            decimal grossPay = totalBasicPay + totalAllowances;

            decimal totalDeductions = empLines
                                .Where(p => p.PayrollType == PayrollType.Deduction)
                                .Sum(e => e.Amount);

            decimal taxDeduction = empLines
                                .Where(p => p.PayrollType == PayrollType.TaxDeduction)
                                .Sum(e => e.Amount);

            decimal netPay = grossPay - totalDeductions - taxDeduction;
        }
    }
}
