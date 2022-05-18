﻿using Application.Contracts.DTOs;
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
            try
            {
                var payrollItem = await _unitOfWork.PayrollItem.Add(_mapper.Map<PayrollItem>(entity));
                await _unitOfWork.SaveAsync();

                //for (int i = 0; i < entity.EmployeeIds.Length; i++)
                //{
                //    await _unitOfWork.PayrollItemEmployee.Add(new PayrollItemEmployee(entity.EmployeeIds[i], payrollItem.Id));
                //    await _unitOfWork.SaveAsync();
                //}

                var assignEmp = new List<PayrollItemEmployee>();

                //assigning EmployeeIds to payrollItems
                for (int i = 0; i < entity.EmployeeIds.Length; i++)
                {
                    assignEmp.Add(new PayrollItemEmployee(entity.EmployeeIds[i], payrollItem.Id));
                }
                await _unitOfWork.PayrollItemEmployee.AddRange(assignEmp);
                await _unitOfWork.SaveAsync();

                //Commiting the transaction 
                _unitOfWork.Commit();

                return new Response<PayrollItemDto>(_mapper.Map<PayrollItemDto>(payrollItem), "Payroll created successfully");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new Response<PayrollItemDto>(ex.Message);
            }
        }

        public async Task<PaginationResponse<List<PayrollItemDto>>> GetAllAsync(PaginationFilter filter)
        {
            var specification = new PayrollItemSpecs(filter);
            var payrollItem = await _unitOfWork.PayrollItem.GetAll(specification);

            if (payrollItem.Count() == 0)
                return new PaginationResponse<List<PayrollItemDto>>("List is empty");

            var totalRecords = await _unitOfWork.PayrollItem.TotalRecord();

            return new PaginationResponse<List<PayrollItemDto>>(_mapper.Map<List<PayrollItemDto>>(payrollItem), filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<PayrollItemDto>> GetByIdAsync(int id)
        {
            //getting payrollItm
            var getPayrollItem = await _unitOfWork.PayrollItem.GetById(id);
            if (getPayrollItem == null)
                return new Response<PayrollItemDto>("Not found");

            var payrollItem = _mapper.Map<PayrollItemDto>(getPayrollItem);

            //getting employeelist in payrollItem
            var empList = _unitOfWork.PayrollItemEmployee
                .Find(new PayrollItemEmployeeSpecs(payrollItem.Id))
                .Select(x => x.Employee)
                .ToList();

            payrollItem.Employees = _mapper.Map<List<EmployeeDto>>(empList);

            return new Response<PayrollItemDto>(payrollItem, "Returning value");
        }

        public async Task<Response<PayrollItemDto>> UpdateAsync(CreatePayrollItemDto entity)
        {
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
            try
            {
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
                    assignEmp.Add(new PayrollItemEmployee(entity.EmployeeIds[i], payrollItem.Id));
                }
                await _unitOfWork.PayrollItemEmployee.AddRange(assignEmp);
                await _unitOfWork.SaveAsync();

                //Commiting the transaction 
                _unitOfWork.Commit();

                return new Response<PayrollItemDto>(_mapper.Map<PayrollItemDto>(payrollItem), "Updated successfully");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new Response<PayrollItemDto>(ex.Message);
            }
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}