using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Helper;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PayrollTransactionService : IPayrollTransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PayrollTransactionService(IUnitOfWork unitOfWork, IMapper mapper, IEmployeeService employeeService, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _employeeService = employeeService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Response<PayrollTransactionDto>> CreateAsync(CreatePayrollTransactionDto entity)
        {
            if (entity.isSubmit)
            {
                return await this.SubmitPayrollTransaction(entity);
            }
            else
            {
                return await this.SavePayrollTransaction(entity, 1);
            }
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PaginationResponse<List<PayrollTransactionDto>>> GetAllAsync(PaginationFilter filter)
        {
            var specification = new PayrollTransactionSpecs(filter);
            var payrollTransactions = await _unitOfWork.PayrollTransaction.GetAll(specification);

            if (payrollTransactions.Count() == 0)
                return new PaginationResponse<List<PayrollTransactionDto>>("List is empty");

            var totalRecords = await _unitOfWork.PayrollTransaction.TotalRecord();

            var response = new List<PayrollTransactionDto>();
            foreach (var i in payrollTransactions)
            {
                response.Add(MapToValue(i));
            }
            return new PaginationResponse<List<PayrollTransactionDto>>(_mapper.Map<List<PayrollTransactionDto>>(response),
                filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<PayrollTransactionDto>> GetByIdAsync(int id)
        {
            var specification = new PayrollTransactionSpecs(false);
            var payrollTransaction = await _unitOfWork.PayrollTransaction.GetById(id, specification);
            if (payrollTransaction == null)
                return new Response<PayrollTransactionDto>("Not found");

            var payrollTransactionDto = _mapper.Map<PayrollTransactionDto>(payrollTransaction);

            payrollTransactionDto.IsAllowedRole = false;
            var workflow = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.PayrollTransaction)).FirstOrDefault();


            if (workflow != null)
            {
                var transition = workflow.WorkflowTransitions
                    .FirstOrDefault(x => (x.CurrentStatusId == payrollTransaction.StatusId));

                if (transition != null)
                {
                    var currentUserRoles = new GetUser(this._httpContextAccessor).GetCurrentUserRoles();
                    foreach (var role in currentUserRoles)
                    {
                        if (transition.AllowedRole.Name == role)
                        {
                            payrollTransactionDto.IsAllowedRole = true;
                        }
                    }
                }
            }

            var result = MapToValue(payrollTransaction);

            return new Response<PayrollTransactionDto>(result, "Returning value");
        }

        private async Task<Response<PayrollTransactionDto>> UpdatePayrollTransaction(CreatePayrollTransactionDto entity, int status)
        {
            if (entity.WorkingDays < entity.PresentDays)
                return new Response<PayrollTransactionDto>("Present Days can not be greater than working days");
            _unitOfWork.CreateTransaction();
            try
            {
                var specification = new PayrollTransactionSpecs(true);
                var getPayrollTransaction = await _unitOfWork.PayrollTransaction.GetById((int)entity.Id, specification);

                if (getPayrollTransaction == null)
                    return new Response<PayrollTransactionDto>("Payroll Transaction with the input id cannot be found");

                var emp = await _employeeService.GetByIdAsync(entity.EmployeeId);

                var empDetails = emp.Result;

                if (empDetails == null)
                    return new Response<PayrollTransactionDto>("Selected employee record not found");

                if (!empDetails.isActive)
                    return new Response<PayrollTransactionDto>("Selected employee is not Active");

                var payrollItemList = _unitOfWork.PayrollItemEmployee
                .Find(new PayrollItemEmployeeSpecs(empDetails.Id, false))
                .Where(x => x.PayrollItem.IsActive == true
                && (x.PayrollType != PayrollType.BasicPay || x.PayrollType != PayrollType.Increment))
                .Select(i => i.PayrollItem)
                .ToList();

                var payrollTransactionLines = payrollItemList
                       .Select(line => new PayrollTransactionLines(line.Id,
                       line.PayrollType,
                       CalculateAllowance(line, entity.WorkingDays, entity.PresentDays, entity.LeaveDays, empDetails.TotalBasicPay),
                       line.AccountId)
                      ).ToList();

                decimal totalAllowances = Math.Round(payrollTransactionLines
                               .Where(p => p.PayrollType == PayrollType.Allowance || p.PayrollType == PayrollType.AssignmentAllowance)
                               .Sum(e => e.Amount), 2);

                decimal totalBasicPay = entity.LeaveDays > 0 ?
                    Math.Round((empDetails.TotalBasicPay / entity.WorkingDays) * (entity.PresentDays + entity.LeaveDays), 2) :
                    Math.Round((empDetails.TotalBasicPay / entity.WorkingDays) * entity.PresentDays, 2);

                decimal grossPay = totalBasicPay + totalAllowances;

                decimal totalDeductions = Math.Round(payrollTransactionLines
                                    .Where(p => p.PayrollType == PayrollType.Deduction || p.PayrollType == PayrollType.TaxDeduction)
                                    .Sum(e => e.Amount), 2);

                decimal netPay = grossPay - totalDeductions;

                // updating data in payroll transaction master table

                _mapper.Map<CreatePayrollTransactionDto, PayrollTransactionMaster>(entity, getPayrollTransaction);

                await _unitOfWork.SaveAsync();

                //Checking month year and emp id
                var checkingPayrollTransSpan = _unitOfWork.PayrollTransaction.Find(new PayrollTransactionSpecs(getPayrollTransaction.Month, getPayrollTransaction.Year, getPayrollTransaction.EmployeeId))
                    .Where(i => (i.Month == getPayrollTransaction.Month) && (i.Year == getPayrollTransaction.Year) && (i.EmployeeId == getPayrollTransaction.EmployeeId))
                    .ToList();

                if (checkingPayrollTransSpan.Count > 1)
                    return new Response<PayrollTransactionDto>("Payroll transaction is already processed");

                //Commiting the transaction 
                _unitOfWork.Commit();
                //returning response
                return new Response<PayrollTransactionDto>(_mapper.Map<PayrollTransactionDto>(getPayrollTransaction), "Created successfully");
            }
            catch (DbUpdateException ex)
            {
                _unitOfWork.Rollback();
                if (ex.InnerException.Data["HelpLink.EvtID"].ToString() == "2627")
                {
                    return new Response<PayrollTransactionDto>("Payroll transaction is already processed");
                }

                return new Response<PayrollTransactionDto>(ex.Message);
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new Response<PayrollTransactionDto>(ex.Message);
            }

        }

        private async Task<Response<PayrollTransactionDto>> SavePayrollTransaction(CreatePayrollTransactionDto entity, int status)
        {
            if (entity.WorkingDays < entity.PresentDays || entity.WorkingDays < entity.PresentDays + entity.LeaveDays)
                return new Response<PayrollTransactionDto>("Present days and Leaves days sum can not be greater than working days");

            _unitOfWork.CreateTransaction();
            try
            {
                //Fetching Employees by id
                var emp = await _employeeService.GetByIdAsync(entity.EmployeeId);

                var empDetails = emp.Result;

                if (empDetails == null)
                    return new Response<PayrollTransactionDto>("Selected employee record not found");

                if (!empDetails.isActive)
                    return new Response<PayrollTransactionDto>("Selected employee is not Active");

                var checkingPayrollTrans = _unitOfWork.PayrollTransaction.Find(new PayrollTransactionSpecs(entity.Year, entity.Month, entity.EmployeeId)).FirstOrDefault();

                if (checkingPayrollTrans != null)
                    return new Response<PayrollTransactionDto>("Payroll transaction is already processed");

               //getting payrollItems by empId
                var payrollItemList = _unitOfWork.PayrollItemEmployee
                .Find(new PayrollItemEmployeeSpecs(empDetails.Id, false))
                .Where(x => ((x.PayrollItem.IsActive == true) && (x.PayrollType != PayrollType.BasicPay && x.PayrollType != PayrollType.Increment)))
                .Select(i => i.PayrollItem)
                .ToList();

                //Calculating allowance by working days 
                var payrollTransactionLines = payrollItemList
                       .Select(line => new PayrollTransactionLines(line.Id,
                       line.PayrollType,
                       CalculateAllowance(line, entity.WorkingDays, entity.PresentDays, entity.LeaveDays, empDetails.TotalBasicPay),
                       line.AccountId)
                      ).ToList();

                decimal totalAllowances = Math.Round(payrollTransactionLines
                               .Where(p => p.PayrollType == PayrollType.Allowance || p.PayrollType == PayrollType.AssignmentAllowance)
                               .Sum(e => e.Amount), 2);

                decimal totalBasicPay = entity.LeaveDays > 0 ?
                    Math.Round((empDetails.TotalBasicPay / entity.WorkingDays) * (entity.PresentDays + entity.LeaveDays), 2) :
                    Math.Round((empDetails.TotalBasicPay / entity.WorkingDays) * entity.PresentDays, 2);

                decimal grossPay = totalBasicPay + totalAllowances;

                decimal totalDeductions = Math.Round(payrollTransactionLines
                                    .Where(p => p.PayrollType == PayrollType.Deduction || p.PayrollType == PayrollType.TaxDeduction)
                                    .Sum(e => e.Amount), 2);

                decimal netPay = grossPay - totalDeductions;

                // mapping data in payroll transaction master table
                var payrollTransaction = new PayrollTransactionMaster(
                    entity.Month,
                    entity.Year,
                    entity.EmployeeId,
                    empDetails.DesignationId,
                    empDetails.DepartmentId,
                    entity.AccountPayableId,
                    entity.WorkingDays,
                    entity.PresentDays,
                    entity.LeaveDays,
                    entity.TransDate,
                    0,
                    empDetails.BasicPay,
                    empDetails.GrossPay,
                    empDetails.NetPay,
                    status,
                    null,
                    payrollTransactionLines);

                await _unitOfWork.PayrollTransaction.Add(payrollTransaction);
                await _unitOfWork.SaveAsync();

                //For creating docNo
                payrollTransaction.CreateDocNo();
                await _unitOfWork.SaveAsync();

                //Commiting the transaction 
                _unitOfWork.Commit();
                //returning response
                return new Response<PayrollTransactionDto>(_mapper.Map<PayrollTransactionDto>(payrollTransaction), "Updated successfully");
            }
            catch (DbUpdateException ex)
            {
                _unitOfWork.Rollback();
                if (ex.InnerException.Data["HelpLink.EvtID"].ToString() == "2627")
                {
                    return new Response<PayrollTransactionDto>("Payroll transaction is already processed");
                }

                return new Response<PayrollTransactionDto>(ex.Message);
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new Response<PayrollTransactionDto>(ex.Message);
            }
        }

        private async Task<Response<PayrollTransactionDto>> SubmitPayrollTransaction(CreatePayrollTransactionDto entity)
        {
            var checkingActiveWorkFlows = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.PayrollTransaction)).FirstOrDefault();

            if (checkingActiveWorkFlows == null)
            {
                return new Response<PayrollTransactionDto>("No workflow found for Payroll Transaction");
            }

            if (entity.Id == null)
            {
                return await this.SavePayrollTransaction(entity, 6);
            }
            else
            {
                return await this.UpdatePayrollTransaction(entity, 6);
            }
        }

        private decimal CalculateAllowance(PayrollItem line, int workingDays, int presentDays, int leaveDays, decimal totalBasicPay)
        {

            var newValue = (line.PayrollItemType == CalculationType.FixedAmount) ? (decimal)(line.Value) :
                  (totalBasicPay * (int)(line.Value) / 100);
            if (line.PayrollType == PayrollType.Allowance || line.PayrollType == PayrollType.AssignmentAllowance)
            {
                if (line.PayrollType == PayrollType.Allowance)
                {
                    if (leaveDays > 0)
                    {
                        return Math.Round((newValue / workingDays) * (presentDays + leaveDays), 2);
                    }
                    return Math.Round((newValue / workingDays) * presentDays, 2);
                }

                if (line.PayrollType == PayrollType.AssignmentAllowance)
                {
                    return Math.Round((newValue / workingDays) * presentDays, 2);
                }
            }
            return newValue;
        }

        private async Task AddToLedger(PayrollTransactionMaster payrollTransaction)
        {
            var transaction = new Transactions(payrollTransaction.DocNo, DocType.PayrollTransaction);
            await _unitOfWork.Transaction.Add(transaction);
            await _unitOfWork.SaveAsync();

            payrollTransaction.setTransactionId(transaction.Id);
            await _unitOfWork.SaveAsync();

            //Inserting line amount into recordledger table
            foreach (var line in payrollTransaction.PayrollTransactionLines)
            {
                var addSalesAmountInRecordLedger = new RecordLedger(
                    transaction.Id,
                    line.AccountId,
                    payrollTransaction.EmployeeId, //need to confirm
                    null,
                    line.PayrollItem.Name,
                    'D',
                    payrollTransaction.BasicSalary,
                    null,
                    payrollTransaction.TransDate
                    );

                await _unitOfWork.Ledger.Add(addSalesAmountInRecordLedger);
                await _unitOfWork.SaveAsync();

                var getCustomerAccount = await _unitOfWork.BusinessPartner.GetById(payrollTransaction.EmployeeId);
                var addReceivableInLedger = new RecordLedger(
                            transaction.Id,
                            getCustomerAccount.AccountReceivableId,
                            payrollTransaction.EmployeeId,
                            null,
                            payrollTransaction.DocNo,
                            'C',
                            line.Amount,
                            null,
                            payrollTransaction.TransDate
                        );

                await _unitOfWork.Ledger.Add(addReceivableInLedger);
                await _unitOfWork.SaveAsync();
            }
        }

        public async Task<Response<bool>> CheckWorkFlow(ApprovalDto data)
        {
            var getPayrollTransaction = await _unitOfWork.PayrollTransaction.GetById(data.DocId, new PayrollTransactionSpecs(true));

            if (getPayrollTransaction == null)
            {
                return new Response<bool>("PayrollTransaction with the input id not found");
            }
            if (getPayrollTransaction.Status.State == DocumentStatus.Unpaid || getPayrollTransaction.Status.State == DocumentStatus.Partial || getPayrollTransaction.Status.State == DocumentStatus.Paid)
            {
                return new Response<bool>("PayrollTransaction already approved");
            }
            var workflow = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.PayrollTransaction)).FirstOrDefault();

            if (workflow == null)
            {
                return new Response<bool>("No activated workflow found for this document");
            }
            var transition = workflow.WorkflowTransitions
                    .FirstOrDefault(x => (x.CurrentStatusId == getPayrollTransaction.StatusId && x.Action == data.Action));

            if (transition == null)
            {
                return new Response<bool>("No transition found");
            }
            var currentUserRoles = new GetUser(this._httpContextAccessor).GetCurrentUserRoles();
            _unitOfWork.CreateTransaction();
            try
            {
                foreach (var role in currentUserRoles)
                {
                    if (transition.AllowedRole.Name == role)
                    {
                        getPayrollTransaction.setStatus(transition.NextStatusId);
                        if (transition.NextStatus.State == DocumentStatus.Unpaid)
                        {
                            await AddToLedger(getPayrollTransaction);
                            _unitOfWork.Commit();
                            return new Response<bool>(true, "PayrollTransaction Approved");
                        }
                        if (transition.NextStatus.State == DocumentStatus.Rejected)
                        {
                            await _unitOfWork.SaveAsync();
                            _unitOfWork.Commit();
                            return new Response<bool>(true, "PayrollTransaction Rejected");
                        }
                        await _unitOfWork.SaveAsync();
                        _unitOfWork.Commit();
                        return new Response<bool>(true, "PayrollTransaction Reviewed");
                    }
                }

                return new Response<bool>("User does not have allowed role");

            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new Response<bool>(ex.Message);
            }
        }

        public async Task<Response<PayrollTransactionDto>> UpdateAsync(CreatePayrollTransactionDto entity)
        {
            if (entity.isSubmit)
            {
                return await this.SubmitPayrollTransaction(entity);
            }
            else
            {
                return await this.UpdatePayrollTransaction(entity, 1);
            }
        }

        private PayrollTransactionDto MapToValue(PayrollTransactionMaster data)
        {
            //For Payroll transaction Lines

            decimal totalAllowances = data.PayrollTransactionLines
                                 .Where(p => p.PayrollType == PayrollType.Allowance || p.PayrollType == PayrollType.AssignmentAllowance)
                                 .Sum(e => e.Amount);


            decimal totalDeductions = data.PayrollTransactionLines
                                 .Where(p => p.PayrollType == PayrollType.Deduction)
                                 .Sum(e => e.Amount);

            decimal taxDeduction = data.PayrollTransactionLines
                                            .Where(p => p.PayrollType == PayrollType.TaxDeduction)
                                            .Sum(e => e.Amount);

            decimal totalAmount = data.NetSalaryBeforeTax;

            //mapping calculated value to employeedto
            var payrollTransactionDto = _mapper.Map<PayrollTransactionDto>(data);

            payrollTransactionDto.TotalAllowances = totalAllowances;
            payrollTransactionDto.TotalDeductions = totalDeductions;
            payrollTransactionDto.TaxDeduction = taxDeduction;
            return payrollTransactionDto;
        }

    }
}
