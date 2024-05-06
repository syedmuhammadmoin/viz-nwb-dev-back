using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Repositories;
using Infrastructure.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PayrollItemService : IPayrollItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;      

        public PayrollItemService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;            
        }

        public async Task<Response<PayrollItemDto>> CreateAsync(CreatePayrollItemDto entity)
        {
            //Checking BasicPay and Increment in amount
            if (entity.PayrollItemType == CalculationType.Percentage)
            {
                if (entity.Value > 100)
                {
                    return new Response<PayrollItemDto>("Percentage should be less than 100%");
                }
            }

            if (entity.PayrollType == PayrollType.BasicPay || entity.PayrollType == PayrollType.Increment)
            {
                if (entity.PayrollItemType == CalculationType.Percentage)
                    return new Response<PayrollItemDto>("Basic pay and increment should be in amount");
            }

            //Checking duplicate employees if any
            var duplicates = entity.EmployeeIds.GroupBy(x => x)
             .Where(g => g.Count() > 1)
             .Select(y => y.Key)
             .ToList();

            if (duplicates.Any())
                return new Response<PayrollItemDto>("Duplicate employees found");


            // with rollback Transaction
            _unitOfWork.CreateTransaction();
          
                var payrollItem = await _unitOfWork.PayrollItem.Add(_mapper.Map<PayrollItem>(entity));
                await _unitOfWork.SaveAsync();

                var assignEmp = new List<PayrollItemEmployee>();

                //assigning EmployeeIds to payrollItems
                for (int i = 0; i < entity.EmployeeIds.Length; i++)
                {
                    if (payrollItem.PayrollType == PayrollType.BasicPay || payrollItem.PayrollType == PayrollType.Increment)
                    {
                        //Checking existing BasicPay/Increments of employee
                        var checkingBasicPayOrIncrement = _unitOfWork.PayrollItemEmployee
                            .Find(new PayrollItemEmployeeSpecs(entity.EmployeeIds[i], payrollItem.PayrollType)).FirstOrDefault();

                        if (checkingBasicPayOrIncrement != null)
                            return new Response<PayrollItemDto>("Basic pay or Increments can not be assigned multiple times");

                        assignEmp.Add(new PayrollItemEmployee(entity.EmployeeIds[i], payrollItem.Id, payrollItem.PayrollType));
                    }
                    else
                    {
                        assignEmp.Add(new PayrollItemEmployee(entity.EmployeeIds[i], payrollItem.Id, payrollItem.PayrollType));
                    }
                }
                await _unitOfWork.PayrollItemEmployee.AddRange(assignEmp);
                await _unitOfWork.SaveAsync();

                //Commiting the transaction 
                _unitOfWork.Commit();

                return new Response<PayrollItemDto>(_mapper.Map<PayrollItemDto>(payrollItem), "Payroll created successfully");
           
        }

        public async Task<PaginationResponse<List<PayrollItemDto>>> GetAllAsync(PayrollItemFilter filter)
        {
            var payrollItemType = new List<CalculationType?>();
            var payrollType = new List<PayrollType?>();
            if (filter.PayrollItemType != null)
            {
                payrollItemType.Add(filter.PayrollItemType);
            }
            if (filter.PayrollType != null)
            {
                payrollType.Add(filter.PayrollType);
            }
            var payrollItem = await _unitOfWork.PayrollItem.GetAll(new PayrollItemSpecs(payrollItemType, payrollType, filter, false));

            if (payrollItem.Count() == 0)
                return new PaginationResponse<List<PayrollItemDto>>(_mapper.Map<List<PayrollItemDto>>(payrollItem), "List is empty");

            var totalRecords = await _unitOfWork.PayrollItem.TotalRecord(new PayrollItemSpecs(payrollItemType, payrollType, filter, true));

            return new PaginationResponse<List<PayrollItemDto>>(_mapper.Map<List<PayrollItemDto>>(payrollItem), filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<PayrollItemDto>> GetByIdAsync(int id)
        {
            //getting payrollItm
            var specification = new PayrollItemSpecs();
            var getPayrollItem = await _unitOfWork.PayrollItem.GetById(id, specification);
            if (getPayrollItem == null)
                return new Response<PayrollItemDto>("Not found");

            var payrollItem = _mapper.Map<PayrollItemDto>(getPayrollItem);

            //getting employeelist in payrollItem
            var empList = _unitOfWork.PayrollItemEmployee
                .Find(new PayrollItemEmployeeSpecs(payrollItem.Id, true))
                .Select(x => x.Employee)
                .ToList();

            payrollItem.Employees = _mapper.Map<List<EmployeeDto>>(empList);

            return new Response<PayrollItemDto>(payrollItem, "Returning value");
        }

        public async Task<Response<PayrollItemDto>> UpdateAsync(CreatePayrollItemDto entity)
        {
            //Checking BasicPay and Increment in amount
            if (entity.PayrollItemType == CalculationType.Percentage)
            {
                if (entity.Value > 100)
                {
                    return new Response<PayrollItemDto>("Percentage should be less than 100%");
                }
            }

            if (entity.PayrollType == PayrollType.BasicPay || entity.PayrollType == PayrollType.Increment)
            {
                if (entity.PayrollItemType == CalculationType.Percentage)
                    return new Response<PayrollItemDto>("Basic pay and increment should be in amount");
            }

            var duplicates = entity.EmployeeIds.GroupBy(x => x)
              .Where(g => g.Count() > 1)
              .Select(y => y.Key)
              .ToList();

            if (duplicates.Any())
                return new Response<PayrollItemDto>("Duplicate employees found");


            // with rollback Transaction
            _unitOfWork.CreateTransaction();
            
                //For updating data
                var payrollItem = await _unitOfWork.PayrollItem.GetById((int)entity.Id);
                _mapper.Map<CreatePayrollItemDto, PayrollItem>(entity, payrollItem);
                await _unitOfWork.SaveAsync();

                //getting and removing all EmployeeIds for this payrollItems
                var checkRemove = await _unitOfWork.PayrollItemEmployee.RemoveAllByPayrollItemId(payrollItem.Id);
                if (!checkRemove)
                {
                    return new Response<PayrollItemDto>("Error updating record");
                }

                var assignEmp = new List<PayrollItemEmployee>();
                //assigning EmployeeIds to payrollItems

                for (int i = 0; i < entity.EmployeeIds.Length; i++)
                {
                    if (payrollItem.PayrollType == PayrollType.BasicPay || payrollItem.PayrollType == PayrollType.Increment)
                    {
                        //Checking existing BasicPay/Increments of employee
                        var checkingBasicPayOrIncrement = _unitOfWork.PayrollItemEmployee
                            .Find(new PayrollItemEmployeeSpecs(entity.EmployeeIds[i], payrollItem.PayrollType)).FirstOrDefault();

                        if (checkingBasicPayOrIncrement != null)
                            return new Response<PayrollItemDto>("Basic pay or Increments can not be assigned multiple times");

                        assignEmp.Add(new PayrollItemEmployee(entity.EmployeeIds[i], payrollItem.Id, payrollItem.PayrollType));
                    }
                    else
                    {
                        assignEmp.Add(new PayrollItemEmployee(entity.EmployeeIds[i], payrollItem.Id, payrollItem.PayrollType));
                    }
                }
                await _unitOfWork.PayrollItemEmployee.AddRange(assignEmp);
                await _unitOfWork.SaveAsync();

                //Commiting the transaction 
                _unitOfWork.Commit();

                return new Response<PayrollItemDto>(_mapper.Map<PayrollItemDto>(payrollItem), "Updated successfully");
           
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<List<PayrollItemDto>>> GetBaicPayList()
        {
            var result = await _unitOfWork.PayrollItem.GetAll(new PayrollItemSpecs(true));
            if (result == null)
                return new Response<List<PayrollItemDto>>("List is empty");

            return new Response<List<PayrollItemDto>>(_mapper.Map<List<PayrollItemDto>>(result), "Returning List");
        }

       
        public Response<List<PayrollResultDto>> GetPayrollItemDropDown(int id)
        {

            var result = _unitOfWork.PayrollItem.GetPayrollItemsByEmployeeId(id);
            if (!result.Any())
                return new Response<List<PayrollResultDto>>("List is empty");

            return new Response<List<PayrollResultDto>>(_mapper.Map<List<PayrollResultDto>>(result), "Returning List");
        }
    }
}
