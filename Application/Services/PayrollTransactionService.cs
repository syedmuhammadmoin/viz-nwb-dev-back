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

        public async Task<PaginationResponse<List<PayrollTransactionDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var docDate = new List<DateTime?>();
            var states = new List<DocumentStatus?>();
            if (filter.DocDate != null)
            {
                docDate.Add(filter.DocDate);
            }
            if (filter.State != null)
            {
                states.Add(filter.State);
            }

            var specification = new PayrollTransactionSpecs(docDate, states, filter);
            var payrollTransactions = await _unitOfWork.PayrollTransaction.GetAll(specification);
            var response = new List<PayrollTransactionDto>();

            if (payrollTransactions.Count() == 0)
                return new PaginationResponse<List<PayrollTransactionDto>>(_mapper.Map<List<PayrollTransactionDto>>(response), "List is empty");

            var totalRecords = await _unitOfWork.PayrollTransaction.TotalRecord();


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
                    .FirstOrDefault(x => (x.CurrentStatusId == payrollTransactionDto.StatusId));

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

        private async Task<Response<PayrollTransactionDto>> SavePayrollTransaction(CreatePayrollTransactionDto entity, int status)
        {
            if (entity.WorkingDays < entity.PresentDays
                || entity.WorkingDays < entity.PresentDays + entity.LeaveDays)
                return new Response<PayrollTransactionDto>("Present days and Leaves days sum can not be greater than working days");

            //Fetching Employees by id
            var emp = await _employeeService.GetByIdAsync(entity.EmployeeId);

            var empDetails = emp.Result;

            if (empDetails == null)
                return new Response<PayrollTransactionDto>("Selected employee record not found");

            if (!empDetails.isActive)
                return new Response<PayrollTransactionDto>("Selected employee is not Active");

            var checkingPayrollTrans = _unitOfWork.PayrollTransaction.Find(new PayrollTransactionSpecs(entity.Month, entity.Year, entity.EmployeeId)).FirstOrDefault();

            if (checkingPayrollTrans != null)
            {
                return new Response<PayrollTransactionDto>("Payroll transaction is already processed");
            }

            //getting payrollItems by empId
            var payrollTransactionLines = empDetails.PayrollItems
            .Where(x => ((x.IsActive == true) && (x.PayrollType != PayrollType.BasicPay && x.PayrollType != PayrollType.Increment)))
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

            _unitOfWork.CreateTransaction();
            try
            {
                // mapping data in payroll transaction master table
                var payrollTransaction = new PayrollTransactionMaster(
                    entity.Month,
                    entity.Year,
                    entity.EmployeeId,
                    empDetails.BPSAccountId,
                    empDetails.BPS,
                    empDetails.DesignationId,
                    empDetails.DepartmentId,
                    entity.AccountPayableId,
                    entity.WorkingDays,
                    entity.PresentDays,
                    entity.LeaveDays,
                    entity.TransDate,
                    totalBasicPay,
                    grossPay,
                    netPay,
                    status,
                    payrollTransactionLines);

                await _unitOfWork.PayrollTransaction.Add(payrollTransaction);
                await _unitOfWork.SaveAsync();

                //For creating docNo
                payrollTransaction.CreateDocNo();
                await _unitOfWork.SaveAsync();

                //Commiting the transaction 
                _unitOfWork.Commit();
                //returning response
                return new Response<PayrollTransactionDto>(null, "Created successfully");
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

                //getting payrollItems by empId
                var payrollTransactionLines = empDetails.PayrollItems
                .Where(x => ((x.IsActive == true) && (x.PayrollType != PayrollType.BasicPay && x.PayrollType != PayrollType.Increment)))
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

                getPayrollTransaction.updatePayrollTransaction(
                    entity.Month,
                    entity.Year,
                    entity.EmployeeId,
                    empDetails.BPSAccountId,
                    empDetails.BPS,
                    empDetails.DesignationId,
                    empDetails.DepartmentId,
                    entity.AccountPayableId,
                    entity.WorkingDays,
                    entity.PresentDays,
                    entity.LeaveDays,
                    entity.TransDate,
                    totalBasicPay,
                    grossPay,
                    netPay,
                    status,
                    payrollTransactionLines);

                await _unitOfWork.SaveAsync();

                //Checking month year and emp id
                var checkingPayrollTransSpan = _unitOfWork.PayrollTransaction.Find(new PayrollTransactionSpecs(getPayrollTransaction.Month, getPayrollTransaction.Year, getPayrollTransaction.EmployeeId))
                    .ToList();

                if (checkingPayrollTransSpan.Count > 1)
                    return new Response<PayrollTransactionDto>("Payroll transaction is already processed");

                //Commiting the transaction 
                _unitOfWork.Commit();
                //returning response
                return new Response<PayrollTransactionDto>(_mapper.Map<PayrollTransactionDto>(getPayrollTransaction), "Updated successfully");
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
                var result = await this.SavePayrollTransaction(entity, 6);
                return new Response<PayrollTransactionDto>(null, result.Message);
            }
            else
            {
                return await this.UpdatePayrollTransaction(entity, 6);
            }
        }

        private decimal CalculateAllowance(PayrollItemDto line, int workingDays, int presentDays, int leaveDays, decimal totalBasicPay)
        {

            if (line.PayrollType == PayrollType.Allowance || line.PayrollType == PayrollType.AssignmentAllowance)
            {
                if (line.PayrollType == PayrollType.Allowance)
                {
                    if (leaveDays > 0)
                    {
                        return Math.Round((line.Value / workingDays) * (presentDays + leaveDays), 2);
                    }
                    return Math.Round((line.Value / workingDays) * presentDays, 2);
                }

                if (line.PayrollType == PayrollType.AssignmentAllowance)
                {
                    return Math.Round((line.Value / workingDays) * presentDays, 2);
                }
            }
            return line.Value;
        }

        private async Task AddToLedger(PayrollTransactionMaster payrollTransaction)
        {
            var transaction = new Transactions(payrollTransaction.DocNo, DocType.PayrollTransaction);
            await _unitOfWork.Transaction.Add(transaction);
            await _unitOfWork.SaveAsync();

            payrollTransaction.setTransactionId(transaction.Id);
            await _unitOfWork.SaveAsync();

            var addBasicPayInLedger = new RecordLedger(
            transaction.Id,
            payrollTransaction.BPSAccountId,
            payrollTransaction.Employee.BusinessPartnerId,
            null,
            payrollTransaction.BPSName,
            'D',
            payrollTransaction.BasicSalary,
            null,
            payrollTransaction.TransDate
            );

            await _unitOfWork.Ledger.Add(addBasicPayInLedger);
            await _unitOfWork.SaveAsync();
            //Inserting line amount into recordledger table

            foreach (var item in payrollTransaction.PayrollTransactionLines)
            {
                //Adding Allowance in ledger
                if (item.PayrollItem.PayrollType == PayrollType.Allowance || item.PayrollItem.PayrollType == PayrollType.AssignmentAllowance)
                {
                    var addAllowanceInRecordLedger = new RecordLedger(
                transaction.Id,
                item.AccountId,
                payrollTransaction.Employee.BusinessPartnerId,
                null,
                item.PayrollItem.Name,
                'D',
                item.Amount,
                null,
                payrollTransaction.TransDate
                );

                    await _unitOfWork.Ledger.Add(addAllowanceInRecordLedger);
                    await _unitOfWork.SaveAsync();
                }
                //Adding Deduction in ledger

                if (item.PayrollItem.PayrollType == PayrollType.Deduction || item.PayrollItem.PayrollType == PayrollType.TaxDeduction)
                {
                    var addDeductionInRecordLedger = new RecordLedger(
                   transaction.Id,
                   item.AccountId,
                   payrollTransaction.Employee.BusinessPartnerId,
                   null,
                   item.PayrollItem.Name,
                   'C',
                   item.Amount,
                   null,
                   payrollTransaction.TransDate
                   );

                    await _unitOfWork.Ledger.Add(addDeductionInRecordLedger);
                    await _unitOfWork.SaveAsync();
                }
            }

            decimal netSalary = payrollTransaction.NetSalary;

            var addPayableInLedger = new RecordLedger(
                transaction.Id,
                payrollTransaction.AccountPayableId,
                payrollTransaction.Employee.BusinessPartnerId,
                null,
                payrollTransaction.DocNo,
                'C',
                netSalary,
                null,
                payrollTransaction.TransDate
                );

            await _unitOfWork.Ledger.Add(addPayableInLedger);
            await _unitOfWork.SaveAsync();
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

        public PayrollTransactionDto MapToValue(PayrollTransactionMaster data)
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

            //mapping calculated value to employeedto
            var payrollTransactionDto = _mapper.Map<PayrollTransactionDto>(data);

            payrollTransactionDto.TotalAllowances = totalAllowances;
            payrollTransactionDto.TotalDeductions = totalDeductions;
            payrollTransactionDto.TaxDeduction = taxDeduction;
            payrollTransactionDto.GrossPay = data.GrossSalary;
            payrollTransactionDto.NetSalary = data.NetSalary;
            payrollTransactionDto.CNIC = data.Employee.CNIC;
            payrollTransactionDto.Religion = data.Employee.Religion;
            payrollTransactionDto.TransDate = data.TransDate;

            var workflow = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.PayrollTransaction)).FirstOrDefault();

            if (workflow != null)
            {
                var transition = workflow.WorkflowTransitions
                    .FirstOrDefault(x => (x.CurrentStatusId == data.StatusId));

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

            payrollTransactionDto.Status = data.Status.Status;

            return payrollTransactionDto;
        }

        public async Task<Response<bool>> ProcessForEdit(int[] id)
        {
            _unitOfWork.CreateTransaction();
            try
            {
                var checkingActiveWorkFlows = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.PayrollTransaction)).FirstOrDefault();

                if (checkingActiveWorkFlows == null)
                {
                    return new Response<bool>("No workflow found for Payroll Transaction");
                }
                for (int i = 0; i < id.Length; i++)
                {
                    var getPayrollTransaction = await _unitOfWork.PayrollTransaction.GetById(id[i]);
                    
                    if (getPayrollTransaction == null)
                    {
                        return new Response<bool>($"Payroll Transaction with the id = {id[i]} not found");
                    }

                    getPayrollTransaction.setStatus(6);

                   await _unitOfWork.SaveAsync();
                }
                _unitOfWork.Commit();

            return new Response<bool>(true, "Payroll transaction submitted successfully");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new Response<bool>(ex.Message);
            }
        }

        public async Task<Response<bool>> ProcessForApproval(CreateApprovalProcessDto data)
        {
            foreach (var docId in data.docId)
            {
                var approval = new ApprovalDto()
                {
                    DocId = docId,
                    Action = data.Action
                };
                var result = await CheckWorkFlow(approval);
                if (!result.IsSuccess)
                {
                    return result;
                }
            }
            return new Response<bool>(true, "Payroll approval process completed successfully");
        }

        public async Task<Response<List<PayrollTransactionDto>>> GetEmployeesByDept(DeptFilter data)
        {
            if (data.AccountPayableId == new Guid("00000000-0000-0000-0000-000000000000"))
            {
                return new Response<List<PayrollTransactionDto>>("Account payable required");
            }

            var createdpayrollTransactions = _unitOfWork.PayrollTransaction
                .Find(new PayrollTransactionSpecs(data.Month, data.Year, data.DepartmentId))
                .ToList();

            if (createdpayrollTransactions.Count() > 0)
            {
                foreach (var transaction in createdpayrollTransactions)
                {
                    var payroll = new CreatePayrollTransactionDto()
                    {
                        Id = transaction.Id,
                        Month = data.Month,
                        Year = data.Year,
                        EmployeeId = transaction.EmployeeId,
                        WorkingDays = transaction.WorkingDays,
                        PresentDays = transaction.PresentDays,
                        TransDate = transaction.TransDate,
                        LeaveDays = transaction.LeaveDays,
                        AccountPayableId = data.AccountPayableId,
                        isSubmit = false,
                    };

                    var result = await this.UpdatePayrollTransaction(payroll, 1);

                    if (result.IsSuccess == false && result.Result == null)
                    {
                        return new Response<List<PayrollTransactionDto>>($"Error creating transaction for {transaction.EmployeeId}");
                    }
                }
            }

            var employeeList = _unitOfWork.Employee
                .Find(new EmployeeSpecs(true, data.DepartmentId)).ToList();

            if (employeeList.Count == 0)
            {
                return new Response<List<PayrollTransactionDto>>("No employee found in this department");
            }


            foreach (var emp in employeeList)
            {
                var payroll = new CreatePayrollTransactionDto()
                {
                    Month = data.Month,
                    Year = data.Year,
                    EmployeeId = emp.Id,
                    WorkingDays = DateTime.DaysInMonth(data.Year, data.Month),
                    PresentDays = DateTime.DaysInMonth(data.Year, data.Month),
                    TransDate = new DateTime(data.Year, data.Month, DateTime.DaysInMonth(data.Year, data.Month)),
                    AccountPayableId = data.AccountPayableId,
                    isSubmit = false,
                };

                var result = await this.SavePayrollTransaction(payroll, 1);

                if (result.IsSuccess == false)
                {
                    return new Response<List<PayrollTransactionDto>>($"Error creating transaction for {emp.Name}");
                    
                }
            }

            var payrollTransactions = _unitOfWork.PayrollTransaction.Find(new PayrollTransactionSpecs(data.Month, data.Year, data.DepartmentId, true)).ToList();
           
            if (payrollTransactions.Count == 0)
            {
                return new Response<List<PayrollTransactionDto>>("No employee found in this department");
            }

            var response = new List<PayrollTransactionDto>();

            foreach (var i in payrollTransactions)
            {
                response.Add(MapToValue(i));
            }

            return new Response<List<PayrollTransactionDto>>(response, "Returning Payroll Transactions");
        }

        public Response<List<PayrollTransactionDto>> GetPayrollTransactionByDept(DeptFilter data)
        {
            var payrollTransactions = _unitOfWork.PayrollTransaction.Find(new PayrollTransactionSpecs(data.Month, data.Year, data.DepartmentId, false)).ToList();

            if (payrollTransactions.Count == 0)
                return new Response<List<PayrollTransactionDto>>("list is empty");

            var response = new List<PayrollTransactionDto>();

            foreach (var i in payrollTransactions)
            {
                response.Add(MapToValue(i));
            }
            var result = response.Where(e => e.IsAllowedRole == true).OrderBy(i => i.Employee).ToList();

            return new Response<List<PayrollTransactionDto>>(result, "Returning Payroll Transactions");
        }

        public Response<List<PayrollTransactionDto>> GetPayrollReport(PayrollFilter filter)
        {
            filter.FromDate = filter.FromDate.Date;
            filter.ToDate = filter.ToDate.Date;
            var employees = new List<int?>();
            var months = new List<int?>();
            var years = new List<int?>();

            if (filter.EmployeeId != null)
            {
                employees.Add(filter.EmployeeId);
            }
            if (filter.Month != null)
            {
                months.Add(filter.Month);
            }
            if (filter.Year != null)
            {
                years.Add(filter.Year);
            }

            var payrollTransactions = _unitOfWork.PayrollTransaction
                .Find(new PayrollTransactionSpecs(months, years, employees, filter.FromDate, filter.ToDate,
                filter.Designation, filter.Department, filter.BPS))
                .ToList();

            if (payrollTransactions.Count() == 0)
            {
                return new Response<List<PayrollTransactionDto>>(null,"List is empty");
            }

            var response = new List<PayrollTransactionDto>();

            foreach (var i in payrollTransactions)
            {
                response.Add(MapToValue(i));
            }
            return new Response<List<PayrollTransactionDto>>(response.OrderBy(i => i.Employee).ToList(), "Returning List");
        }
    }
}
