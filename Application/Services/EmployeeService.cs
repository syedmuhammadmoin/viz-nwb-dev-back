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
            
            _unitOfWork.CreateTransaction(); 
            try
            {
                var checkCNIC = _unitOfWork.Employee.Find(new EmployeeSpecs(entity.CNIC)).FirstOrDefault();

                if (checkCNIC == null)
                {
                    // mapping businessPartner model
                    var businessPartner = new BusinessPartner(
                        entity.Name,
                        BusinessPartnerType.Employee,
                        entity.CNIC
                        );

                    // Saving BP 
                    var savingBP = await _unitOfWork.BusinessPartner.Add(businessPartner);
                    await _unitOfWork.SaveAsync();

                    // Check if bp is created
                    if (savingBP == null)
                    {
                        _unitOfWork.Rollback();
                        return new Response<EmployeeDto>("Error creating Employee");
                    }

                    var employee = _mapper.Map<Employee>(entity);

                    employee.setBusinessPartnerId(savingBP.Id);

                    await _unitOfWork.Employee.Add(employee);
                    await _unitOfWork.SaveAsync();

                    _unitOfWork.Commit();
                    return new Response<EmployeeDto>(_mapper.Map<EmployeeDto>(employee), "Created successfully");
                }
                
                _mapper.Map<CreateEmployeeDto, Employee>(entity, checkCNIC);
                await _unitOfWork.SaveAsync();
                _unitOfWork.Commit();
                return new Response<EmployeeDto>(_mapper.Map<EmployeeDto>(checkCNIC), "updated successfully");

            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new Response<EmployeeDto>(ex.Message);
            }
        }

        public async Task<PaginationResponse<List<EmployeeDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var specification = new EmployeeSpecs(filter);
            var employees = await _unitOfWork.Employee.GetAll(specification);

            if (employees.Count() == 0)
                return new PaginationResponse<List<EmployeeDto>>(_mapper.Map<List<EmployeeDto>>(employees), "List is empty");

            var totalRecords = await _unitOfWork.Employee.TotalRecord(specification);

            return new PaginationResponse<List<EmployeeDto>>(_mapper.Map<List<EmployeeDto>>(employees), filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<EmployeeDto>> GetByIdAsync(int id)
        {
            var getEmployee = await _unitOfWork.Employee.GetById(id, new EmployeeSpecs());
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
            
            result.PayrollItems = result.PayrollItems
                .Where(i => i.PayrollType != PayrollType.BasicPay
                && i.PayrollType != PayrollType.Increment).ToList();
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
                var increment = data.PayrollItems
                                .Where(p => p.PayrollType == PayrollType.Increment && p.IsActive == true)
                                .FirstOrDefault();
                if (increment != null)
                {
                    incrementAmount = increment.Value;
                    totalIncrement = (incrementAmount * (int)(data.NoOfIncrements));
                }
            }
            
            var basicPayItem = data.PayrollItems
                                .Where(p => p.PayrollType == PayrollType.BasicPay && p.IsActive == true)
                                .FirstOrDefault();

            if (basicPayItem != null)
            {
                decimal totalBasicPay = (basicPayItem.Value + totalIncrement);

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
                data.BasicPay = basicPayItem.Value;
                data.BPS = basicPayItem.Name;
                data.BPSAccountId = basicPayItem.AccountId;
                data.Increment = incrementAmount;
                data.TotalIncrement = totalIncrement;
                data.TotalBasicPay = totalBasicPay;
                data.TotalAllowances = totalAllowances;
                data.GrossPay = grossPay;
                data.TotalDeductions = totalDeductions;
                data.TaxDeduction = taxDeduction;
                data.NetPay = netPay;
            }

            
            return data;
        }

        public async Task<Response<List<EmployeeDropDownPaymentDto>>> GetEmployeeDropDownPayment()
        {
            var specification = new EmployeeSpecs(true);
            var employees = await _unitOfWork.Employee.GetAll(specification);
            if (!employees.Any())
                return new Response<List<EmployeeDropDownPaymentDto>>("List is empty");

            return new Response<List<EmployeeDropDownPaymentDto>>(_mapper.Map<List<EmployeeDropDownPaymentDto>>(employees), "Returning List");
        }


    }
}
