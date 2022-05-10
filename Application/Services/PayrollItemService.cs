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
            if (entity.PayrollType == PayrollType.BasicPay || entity.PayrollType == PayrollType.Increment)
            {
                if (entity.PayrollItemType == CalculationType.Percentage)
                    return new Response<PayrollItemDto>("Basic pay and increment should be in amount");
            }

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

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
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
            var specification = new PayrollItemSpecs();
            var payrollItem = await _unitOfWork.PayrollItem.GetById(id, specification);
            if (payrollItem == null)
                return new Response<PayrollItemDto>("Not found");

            return new Response<PayrollItemDto>(_mapper.Map<PayrollItemDto>(payrollItem), "Returning value");
        }

        public async Task<Response<PayrollItemDto>> UpdateAsync(CreatePayrollItemDto entity)
        {
            if (entity.PayrollType == PayrollType.BasicPay || entity.PayrollType == PayrollType.Increment)
            {
                if (entity.PayrollItemType == CalculationType.Percentage)
                    return new Response<PayrollItemDto>("Basic pay and increment should be in amount");
            }
            _unitOfWork.CreateTransaction();
            try
            {
                var GetPayrollItem = _unitOfWork.PayrollItem.Find(new PayrollItemSpecs((int)entity.Id)).FirstOrDefault();

                //For updating data
                _mapper.Map<CreatePayrollItemDto, PayrollItem>(entity, GetPayrollItem);
                await _unitOfWork.SaveAsync();

                //getting and removing all EmployeeIds for this payrollItems
                var empIds = await _unitOfWork.PayrollItemEmployee.get(entity);
                foreach (var claim in claims)
                {
                    await _roleManager.RemoveClaimAsync(role, claim);
                }

                //Commiting the transaction 
                _unitOfWork.Commit();

                return new Response<PayrollItemDto>(_mapper.Map<PayrollItemDto>(GetPayrollItem), "Updated successfully");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new Response<PayrollItemDto>(ex.Message);
            }
        }
    }
}
