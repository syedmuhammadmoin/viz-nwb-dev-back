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
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
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
        private async Task<Response<PayrollTransactionDto>> CreatePayroll(CreatePayrollTransactionDto item)
        {
            if (item.WorkingDays < item.PresentDays
               || item.WorkingDays < item.PresentDays + item.LeaveDays)
                return new Response<PayrollTransactionDto>($"Error in this Employee with CNIC{item.EmployeeCNIC}, Present days and Leaves days sum can not be greater than working days");

            //Fetching Employees by id
            var emp = _employeeService.GetEmpByCNIC(item.EmployeeCNIC);

            var empDetails = emp.Result;

            if (empDetails == null)
                return new Response<PayrollTransactionDto>($"Error in this Employee with CNIC{item.EmployeeCNIC},Selected employee record not found");

            if (!empDetails.isActive)
                return new Response<PayrollTransactionDto>($"Error in this Employee with CNIC{item.EmployeeCNIC},Selected employee is not Active");

            if (empDetails.BasicPay == 0)
            {
                return new Response<PayrollTransactionDto>($"Error in this Employee with CNIC{item.EmployeeCNIC},Employee basic pay is required");
            }

            var checkingPayrollTrans = _unitOfWork.PayrollTransaction.Find(new PayrollTransactionSpecs((int)item.Month, (int)item.Year, empDetails.Id)).FirstOrDefault();

            if (checkingPayrollTrans != null)
            {
                if (checkingPayrollTrans.StatusId != 1 && checkingPayrollTrans.StatusId != 2)
                    return new Response<PayrollTransactionDto>($"Error in this Employee with CNIC{item.EmployeeCNIC},Payroll transaction is already processed");

                return await UpdatePayroll(checkingPayrollTrans.Id, item, 1);
            }

            //Creating Initial Transaction of a payroll

            //getting payrollItems by empId
            var payrollTransactionLines = empDetails.PayrollItems
            .Where(x => ((x.IsActive == true) && (x.PayrollType != PayrollType.BasicPay && x.PayrollType != PayrollType.Increment)))
            .Select(line => new PayrollTransactionLines(line.Id,
                   line.PayrollType,
                   line.Value,
                   CalculateAllowance(line, (int)item.WorkingDays, (int)item.PresentDays, (int)item.LeaveDays),
                   line.AccountId)
            ).ToList();


            decimal totalAllowances = Math.Round(payrollTransactionLines
                           .Where(p => p.PayrollType == PayrollType.Allowance || p.PayrollType == PayrollType.AssignmentAllowance)
                           .Sum(e => e.Amount), 2);

            decimal totalBasicPay = item.LeaveDays > 0 ?
                Math.Round((decimal)(empDetails.TotalBasicPay / (int)item.WorkingDays) * ((int)item.PresentDays + (int)item.LeaveDays), 2) :
                Math.Round(((decimal)empDetails.TotalBasicPay / (int)item.WorkingDays) * (int)item.PresentDays, 2);

            decimal grossPay = totalBasicPay + totalAllowances;

            decimal totalDeductions = Math.Round(payrollTransactionLines
                                .Where(p => p.PayrollType == PayrollType.Deduction || p.PayrollType == PayrollType.TaxDeduction)
                                .Sum(e => e.Amount), 2);

            decimal netPay = grossPay - totalDeductions;

            if (netPay < 0)
                return new Response<PayrollTransactionDto>($"Net salary for Employee with CNIC{item.EmployeeCNIC} shouldn't be less than deductions ");


            _unitOfWork.CreateTransaction();
            try
            {
                var payrollTransaction = new PayrollTransactionMaster(
                    (int)item.Month,
                    (int)item.Year,
                    empDetails.BPSAccountId,
                    empDetails.BPS,
                    empDetails.CampusId,
                    (int)item.WorkingDays,
                    (int)item.PresentDays,
                    (int)item.LeaveDays,
                    (DateTime)item.TransDate,
                    totalBasicPay,
                    grossPay,
                    netPay,
                    1,
                    empDetails.Id,
                    empDetails.Name,
                    empDetails.FatherName,
                    empDetails.CNIC,
                    empDetails.EmployeeType,
                    empDetails.BankName,
                    empDetails.BranchName,
                    empDetails.AccountTitle,
                    empDetails.AccountNumber,
                    empDetails.EmployeeCode,
                    empDetails.Domicile,
                    empDetails.Contact,
                    empDetails.Religion,
                    empDetails.Nationality,
                    empDetails.Maritalstatus,
                    empDetails.Gender,
                    empDetails.PlaceofBirth,
                    empDetails.DesignationId,
                    empDetails.DepartmentId,
                    empDetails.Address,
                    empDetails.DateofJoining,
                    empDetails.DateofRetirment,
                    empDetails.DateofBirth,
                    empDetails.Faculty,
                    empDetails.DutyShift,
                    empDetails.NoOfIncrements,
                    empDetails.Email,
                    empDetails.BasicPayItemId,
                    empDetails.BasicPay,
                    empDetails.IncrementItemId,
                    empDetails.IncrementName,
                    empDetails.IncrementAmount,
                    payrollTransactionLines
                    );

                await _unitOfWork.PayrollTransaction.Add(payrollTransaction);
                await _unitOfWork.SaveAsync();

                //For creating docNo
                payrollTransaction.CreateDocNo();
                await _unitOfWork.SaveAsync();

                //Commiting the transaction 
                _unitOfWork.Commit();
                //returning response
                return new Response<PayrollTransactionDto>(_mapper.Map<PayrollTransactionDto>(payrollTransaction), "Created successfully");
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
        private async Task<Response<PayrollTransactionDto>> UpdatePayroll(int id, CreatePayrollTransactionDto entity, int status)
        {
            var emp = _employeeService.GetEmpByCNIC(entity.EmployeeCNIC);

            var empDetails = emp.Result;

            //getting payrollItems by empId
            var payrollTransactionLines = empDetails.PayrollItems
            .Where(x => ((x.IsActive == true) && (x.PayrollType != PayrollType.BasicPay && x.PayrollType != PayrollType.Increment)))
            .Select(line => new PayrollTransactionLines(line.Id,
                   line.PayrollType,
                   line.Value,
                   CalculateAllowance(line, (int)entity.WorkingDays, (int)entity.PresentDays, (int)entity.LeaveDays),
                   line.AccountId)
            ).ToList();

            decimal totalAllowances = Math.Round(payrollTransactionLines
                           .Where(p => p.PayrollType == PayrollType.Allowance || p.PayrollType == PayrollType.AssignmentAllowance)
                           .Sum(e => e.Amount), 2);

            decimal totalBasicPay = entity.LeaveDays > 0 ?
                Math.Round((decimal)(empDetails.TotalBasicPay / (int)entity.WorkingDays) * ((int)entity.PresentDays + (int)entity.LeaveDays), 2) :
                Math.Round((decimal)(empDetails.TotalBasicPay / (int)entity.WorkingDays) * (int)entity.PresentDays, 2);

            decimal grossPay = totalBasicPay + totalAllowances;

            decimal totalDeductions = Math.Round(payrollTransactionLines
                                .Where(p => p.PayrollType == PayrollType.Deduction || p.PayrollType == PayrollType.TaxDeduction)
                                .Sum(e => e.Amount), 2);

            decimal netPay = grossPay - totalDeductions;

            if (netPay < 0)
                return new Response<PayrollTransactionDto>($"Net salary for Employee with CNIC{entity.EmployeeCNIC} shouldn't be less than deductions ");

            _unitOfWork.CreateTransaction();

            // updating data in payroll transaction master table
            var getPayrollTransaction = await _unitOfWork.PayrollTransaction.GetById(id, new PayrollTransactionSpecs(true));

            if (getPayrollTransaction == null)
                return new Response<PayrollTransactionDto>("Payroll Transaction with the input id cannot be found");

            getPayrollTransaction.UpdatePayrollTransaction(
                    empDetails.BPSAccountId,
                    empDetails.BPS,
                    empDetails.CampusId,
                    (int)entity.WorkingDays,
                    (int)entity.PresentDays,
                    (int)entity.LeaveDays,
                    (DateTime)entity.TransDate,
                    totalBasicPay,
                    grossPay,
                    netPay,
                    1,
                    empDetails.Name,
                    empDetails.FatherName,
                    empDetails.EmployeeType,
                    empDetails.BankName,
                    empDetails.BranchName,
                    empDetails.AccountTitle,
                    empDetails.AccountNumber,
                    empDetails.EmployeeCode,
                    empDetails.Domicile,
                    empDetails.Contact,
                    empDetails.Religion,
                    empDetails.Nationality,
                    empDetails.Maritalstatus,
                    empDetails.Gender,
                    empDetails.PlaceofBirth,
                    empDetails.DesignationId,
                    empDetails.DepartmentId,
                    empDetails.Address,
                    empDetails.DateofJoining,
                    empDetails.DateofRetirment,
                    empDetails.DateofBirth,
                    empDetails.Faculty,
                    empDetails.DutyShift,
                    empDetails.NoOfIncrements,
                    empDetails.Email,
                    empDetails.BasicPayItemId,
                    empDetails.BasicPay,
                    empDetails.IncrementItemId,
                    empDetails.IncrementName,
                    empDetails.IncrementAmount,
                    payrollTransactionLines
                );

            await _unitOfWork.SaveAsync();
            //Commiting the transaction 
            _unitOfWork.Commit();
            //returning response
            return new Response<PayrollTransactionDto>(_mapper.Map<PayrollTransactionDto>(getPayrollTransaction), "Updated successfully");
        }        
        public async Task<Response<PayrollTransactionDto>> UpdatePayrollTransaction(int id, UpdateEmployeeTransactionDto entity,int status)
        {
            //_unitOfWork.CreateTransaction();

            //var getPayrollTransaction = await _unitOfWork.PayrollTransaction.GetById(id, new PayrollTransactionSpecs(true));
            //if (getPayrollTransaction == null)
            //    return new Response<PayrollTransactionDto>("Payroll Transaction with the input id cannot be found");

            ////For updating data
            //getPayrollTransaction.UpdatePayrollTransaction(    
            //    entity.Religion,

            //    entity.CNIC,
            //    entity.Month,
            //    entity.Year,
            //    entity.EmployeeId,

            //    entity.DesignationId,
            //    entity.DepartmentId,
            //    entity.CampusId,
            //    (int)entity.WorkingDays,
            //    (int)entity.PresentDays,
            //    (int)entity.LeaveDays,
            //    (DateTime)entity.TransDate,
            //    entity.BasicSalary,
            //    entity.grossPay,
            //    entity.NetSalary,                   
            //    1,
            //    entity.EmployeeType,                                        
            //    entity.payrollTransactionLines
            //    );

            //await _unitOfWork.SaveAsync();
            ////Commiting the transaction 
            //_unitOfWork.Commit();
            ////returning response
            //return new Response<PayrollTransactionDto>(_mapper.Map<PayrollTransactionDto>(getPayrollTransaction), "Updated successfully");

            var result = await _unitOfWork.PayrollTransaction.GetById((int)entity.Id);
            if (result == null)
                return new Response<PayrollTransactionDto>("Not found");

            //For updating data
            _mapper.Map(entity, result);
            await _unitOfWork.SaveAsync();
            return new Response<PayrollTransactionDto>(_mapper.Map<PayrollTransactionDto>(result), "Updated successfully");
        }
        private async Task<Response<PayrollTransactionDto>> UpdatePayrollTransaction(UpdatePayrollTransactionDto entity, int status)
        {
            var getPayrollTransaction = await _unitOfWork.PayrollTransaction.GetById((int)entity.Id);

            if (getPayrollTransaction == null)
                return new Response<PayrollTransactionDto>("Payroll Transaction with the input id cannot be found");

            // updating data in payroll transaction master table
            getPayrollTransaction.UpdateAccountPayableId((Guid)entity.AccountPayableId, status);
            await _unitOfWork.SaveAsync();

            //returning response
            return new Response<PayrollTransactionDto>(null, "Updated successfully");
        }
        private async Task<Response<PayrollTransactionDto>> SubmitPayrollTransaction(UpdatePayrollTransactionDto entity)
        {
            var checkingActiveWorkFlows = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.PayrollTransaction)).FirstOrDefault();

            if (checkingActiveWorkFlows == null)
                return new Response<PayrollTransactionDto>("No workflow found for Payroll Transaction");


            return await this.UpdatePayrollTransaction(entity, 6);
        }
        private decimal CalculateAllowance(PayrollItemDto line, int workingDays, int presentDays, int leaveDays)
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
            var transaction = new Transactions(payrollTransaction.Id, payrollTransaction.DocNo, DocType.PayrollTransaction);
            await _unitOfWork.Transaction.Add(transaction);
            await _unitOfWork.SaveAsync();

            payrollTransaction.SetTransactionId(transaction.Id);
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
                (Guid)payrollTransaction.AccountPayableId,
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

            //Getting transaction with Payment Transaction Id
            var getUnreconciledDocumentAmount = _unitOfWork.Ledger.Find(new LedgerSpecs(transaction.Id, true)).FirstOrDefault();
            payrollTransaction.SetLedgerId(getUnreconciledDocumentAmount.Id);
            await _unitOfWork.SaveAsync();

        }
        private List<RemarksDto> ReturningRemarks(PayrollTransactionDto data, DocType docType)
        {
            var remarks = _unitOfWork.Remarks.Find(new RemarksSpecs(data.Id, DocType.PayrollTransaction))
                    .Select(e => new RemarksDto()
                    {
                        Remarks = e.Remarks,
                        UserName = e.User.UserName,
                        CreatedAt = e.CreatedDate == null ? "N/A" : ((DateTime)e.CreatedDate).ToString("ddd, dd MMM yyyy")
                    }).ToList();

            if (remarks.Count() > 0)
            {
                data.RemarksList = _mapper.Map<List<RemarksDto>>(remarks);
            }

            return remarks;
        }
        private List<FileUploadDto> ReturningFiles(PayrollTransactionDto data, DocType docType)
        {

            var files = _unitOfWork.Fileupload.Find(new FileUploadSpecs(data.Id, DocType.PayrollTransaction))
                    .Select(e => new FileUploadDto()
                    {
                        Id = e.Id,
                        Name = e.Name,
                        DocType = DocType.PayrollTransaction,
                        Extension = e.Extension,
                        UserName = e.User.UserName,
                        CreatedAt = e.CreatedDate == null ? "N/A" : ((DateTime)e.CreatedDate).ToString("ddd, dd MMM yyyy")
                    }).ToList();

            if (files.Count() > 0)
            {
                data.FileUploadList = _mapper.Map<List<FileUploadDto>>(files);
            }
            return files;
        }
        private DataTable PayrollItemDetailReport(PayrollDetailFilter filter)
        {

            filter.FromDate = filter.FromDate?.Date;
            filter.ToDate = filter.ToDate?.Date;
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
                .Find(new PayrollTransactionSpecs(months, years, employees, (DateTime)filter.FromDate, (DateTime)filter.ToDate,
                filter.Designation, filter.Department, filter.Campus))
                .ToList();

            if (payrollTransactions.Count() == 0)
            {
                return null;
            }

            var allowanceDTO = new List<PayrollTransactionDto>();




            foreach (var payroll in payrollTransactions)
            {


                if (payroll.PayrollTransactionLines.Count() > 0)
                {
                    foreach (var lines in payroll.PayrollTransactionLines)
                    {
                        allowanceDTO.Add(new PayrollTransactionDto
                        {
                            Employee = payroll.Employee.Name,
                            CNIC = payroll.CNIC,
                            Department = payroll.Department.Name,
                            Campus = payroll.Campus.Name,
                            Designation = payroll.Designation.Name,
                            AccountName = lines.Account.Name,
                            Amount = lines.Amount,
                            NetSalary = payroll.NetSalary,
                            GrossPay = payroll.GrossSalary
                        });
                    }
                }
            }

            allowanceDTO = allowanceDTO
               .GroupBy(x => new { x.Employee, x.NetSalary, x.GrossPay , x.CNIC, x.Department, x.Campus, x.Designation, x.AccountName })
               .Select(c => new PayrollTransactionDto
               {
                   Employee = c.Key.Employee,
                   CNIC = c.Key.CNIC,
                   Department = c.Key.Department,
                   Campus = c.Key.Campus,
                   Designation = c.Key.Designation,
                   AccountName = c.Key.AccountName,
                   NetSalary = c.Key.NetSalary,
                   GrossPay = c.Key.GrossPay,
                   Amount = c.Sum(e => e.Amount)
               })
               .ToList();

            var groups = from d in allowanceDTO
                         group d by new { d.Employee, d.NetSalary, d.GrossPay,  d.CNIC, d.Department, d.Campus, d.Designation }
                        into grp
                         select new
                         {
                             Employee = grp.Key.Employee,
                             CNIC = grp.Key.CNIC,
                             Department = grp.Key.Department,
                             Campus = grp.Key.Campus,
                             NetSalary = grp.Key.NetSalary,
                             GrossPay = grp.Key.GrossPay,
                             Designation = grp.Key.Designation,
                             Items = grp.Select(d2 => new { d2.AccountName, d2.Amount }).ToArray()
                         };

            /*get all possible subjects into a separate group*/
            var itemNames = (from d in allowanceDTO
                             select d.AccountName).Distinct();


            DataTable dt = new DataTable();
            /*for static cols*/
            dt.Columns.Add("Employee");
            dt.Columns.Add("CNIC");
            dt.Columns.Add("NetSalary");
            dt.Columns.Add("GrossPay");
            dt.Columns.Add("Department");
            dt.Columns.Add("Campus");
            dt.Columns.Add("Designation");
            /*for dynamic cols*/
            foreach (var item in itemNames)
            {
                dt.Columns.Add(item.ToString());
            }

            /*pivot the data into a new datatable*/
            foreach (var g in groups)
            {
                DataRow dr = dt.NewRow();
                dr["Employee"] = g.Employee;
                dr["CNIC"] = g.CNIC;
                dr["NetSalary"] = g.NetSalary;
                dr["GrossPay"] = g.GrossPay;
                dr["Department"] = g.Department;
                dr["Campus"] = g.Campus;
                dr["Designation"] = g.Designation;

                foreach (var item in g.Items)
                {
                    dr[item.AccountName] = item.Amount;
                }
                dt.Rows.Add(dr);
            }
            return dt;

        }
        public PayrollTransactionService(IUnitOfWork unitOfWork, IMapper mapper, IEmployeeService employeeService, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _employeeService = employeeService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Response<PayrollTransactionDto>> CreateAsync(CreatePayrollTransactionDto[] entity)
        {

            foreach (var item in entity)
            {
                var result = await CreatePayroll(item);
                if (!result.IsSuccess)
                {
                    return result;
                }
            }
            return new Response<PayrollTransactionDto>(null, "Records populated successfully");

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
            var payrollTransactions = await _unitOfWork.PayrollTransaction.GetAll(new PayrollTransactionSpecs(docDate, states, filter, false));
            var response = new List<PayrollTransactionDto>();

            if (payrollTransactions.Count() == 0)
                return new PaginationResponse<List<PayrollTransactionDto>>(_mapper.Map<List<PayrollTransactionDto>>(response), "List is empty");

            var totalRecords = await _unitOfWork.PayrollTransaction.TotalRecord(new PayrollTransactionSpecs(docDate, states, filter, true));


            foreach (var i in payrollTransactions)
            {
                response.Add(MapToValue(i));
            }
            return new PaginationResponse<List<PayrollTransactionDto>>(_mapper.Map<List<PayrollTransactionDto>>(response),
                filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
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
            var getUser = new GetUser(this._httpContextAccessor);

            var userId = getUser.GetCurrentUserId();
            var currentUserRoles = new GetUser(this._httpContextAccessor).GetCurrentUserRoles();
            _unitOfWork.CreateTransaction();

            foreach (var role in currentUserRoles)
            {
                if (transition.AllowedRole.Name == role)
                {
                    if (!String.IsNullOrEmpty(data.Remarks))
                    {
                        var addRemarks = new Remark()
                        {
                            DocId = getPayrollTransaction.Id,
                            DocType = DocType.PayrollTransaction,
                            Remarks = data.Remarks,
                            UserId = userId
                        };
                        await _unitOfWork.Remarks.Add(addRemarks);
                    }

                    getPayrollTransaction.SetStatus(transition.NextStatusId);
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
        public async Task<Response<PayrollTransactionDto>> UpdateAsync(UpdatePayrollTransactionDto entity)
        {
            return await this.SubmitPayrollTransaction(entity);
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

            ReturningRemarks(payrollTransactionDto, DocType.PayrollTransaction);

            ReturningFiles(payrollTransactionDto, DocType.PayrollTransaction);

            payrollTransactionDto.TotalAllowances = totalAllowances;
            payrollTransactionDto.TotalDeductions = totalDeductions;
            payrollTransactionDto.TaxDeduction = taxDeduction;
            payrollTransactionDto.GrossPay = data.GrossSalary;
            payrollTransactionDto.NetSalary = data.NetSalary;
            payrollTransactionDto.CNIC = data.Employee.CNIC;
            payrollTransactionDto.Religion = data.Employee.Religion;
            payrollTransactionDto.TransDate = data.TransDate;

            if ((data.Status.State == DocumentStatus.Unpaid || data.Status.State == DocumentStatus.Partial || data.Status.State == DocumentStatus.Paid) && data.TransactionId != null)
            {
                //Getting transaction with Payment Transaction Id
                var getUnreconciledDocumentAmount = _unitOfWork.Ledger.Find(new LedgerSpecs((int)data.TransactionId, true)).FirstOrDefault();

                // Checking if given amount is greater than unreconciled document amount
                var transactionReconciles = _unitOfWork.TransactionReconcile.Find(new TransactionReconSpecs(getUnreconciledDocumentAmount.Id, false)).ToList();

                //For Paid Document List
                var paidDocList = new List<PaidDocListDto>();
                // if reconciles transaction found
                if (transactionReconciles.Count() > 0)
                {
                    //Adding Paid Doc List
                    foreach (var tranRecon in transactionReconciles)
                    {
                        paidDocList.Add(new PaidDocListDto
                        {
                            Id = tranRecon.PaymentLedger.Transactions.DocId,
                            DocNo = tranRecon.PaymentLedger.Transactions.DocNo,
                            DocType = tranRecon.PaymentLedger.Transactions.DocType,
                            Amount = tranRecon.Amount
                        });
                    }
                }

                //Getting Pending Invoice Amount
                var pendingAmount = data.NetSalary - transactionReconciles.Sum(e => e.Amount);

                if (data.Status.State != DocumentStatus.Paid)
                {
                    //Creating transactionReconRepo object
                    TransactionReconcileService trasactionReconService = new TransactionReconcileService(_unitOfWork);
                    var getUnreconPayment = trasactionReconService.GetPaymentReconAmounts(getUnreconciledDocumentAmount.Level4_id, (int)getUnreconciledDocumentAmount.BusinessPartnerId, getUnreconciledDocumentAmount.Sign);


                    //For Getting Business Partner Unreconciled Payments and CreditNote
                    var BPUnreconPayments = getUnreconPayment.Result.Select(i => new UnreconciledBusinessPartnerPaymentsDto()
                    {
                        Id = i.DocId,
                        DocNo = i.DocNo,
                        DocType = i.DocType,
                        Amount = i.UnreconciledAmount,
                        PaymentLedgerId = i.PaymentLedgerId
                    }).ToList();

                    payrollTransactionDto.BPUnreconPaymentList = BPUnreconPayments;
                }

                //data.Status = data.State == DocumentStatus.Unpaid ? "Unpaid" : data.Status;
                payrollTransactionDto.TotalPaid = transactionReconciles.Sum(e => e.Amount);
                payrollTransactionDto.PaidAmountList = paidDocList;
                payrollTransactionDto.PendingAmount = pendingAmount;
                payrollTransactionDto.LedgerId = getUnreconciledDocumentAmount.Id;
            }

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
            return payrollTransactionDto;
        }
        public async Task<Response<bool>> ProcessForEdit(int[] id)
        {
            _unitOfWork.CreateTransaction();

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

                getPayrollTransaction.SetStatus(6);

                await _unitOfWork.SaveAsync();
            }
            _unitOfWork.Commit();

            return new Response<bool>(true, "Payroll transaction submitted successfully");



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

            var payrollTransactions = _unitOfWork.PayrollTransaction
                .Find(new PayrollTransactionSpecs((int)data.Month, (int)data.Year, data.DepartmentId, (int)data.CampusId))
                .ToList();

            if (payrollTransactions.Count() > 0)
            {
                foreach (var payrollTransaction in payrollTransactions)
                {
                    var payroll = new UpdatePayrollTransactionDto()
                    {
                        Id = payrollTransaction.Id,
                        AccountPayableId = data.AccountPayableId,
                        isSubmit = false,
                    };

                    var result = await this.UpdatePayrollTransaction(payroll, payrollTransaction.StatusId);

                    if (result.IsSuccess == false && result.Message != "Payroll transaction is already processed")
                    {
                        return new Response<List<PayrollTransactionDto>>($"Error creating transaction for {payrollTransaction.EmployeeId}");
                    }
                }
            }

            var getpayrollTransactions = _unitOfWork.PayrollTransaction.Find(new PayrollTransactionSpecs((int)data.Month, (int)data.Year, data.DepartmentId, (int)data.CampusId, true)).ToList();

            if (getpayrollTransactions.Count == 0)
            {
                return new Response<List<PayrollTransactionDto>>("No employees found");
            }

            var response = new List<PayrollTransactionDto>();

            foreach (var i in getpayrollTransactions)
            {
                response.Add(MapToValue(i));
            }

            return new Response<List<PayrollTransactionDto>>(response, "Returning Payroll Transactions");
        }
        public Response<List<PayrollTransactionDto>> GetPayrollTransactionByDept(DeptFilter data)
        {
            var payrollTransactions = _unitOfWork.PayrollTransaction.Find(new PayrollTransactionSpecs((int)data.Month, (int)data.Year, data.DepartmentId, (int)data.CampusId, false)).ToList();

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
            filter.FromDate = filter.FromDate?.Date;
            filter.ToDate = filter.ToDate?.Date;
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
                .Find(new PayrollTransactionSpecs(months, years, employees, (DateTime)filter.FromDate, (DateTime)filter.ToDate,
                filter.Designation, filter.Department, filter.Campus, filter.BPS))
                .ToList();

            if (payrollTransactions.Count() == 0)
            {
                return new Response<List<PayrollTransactionDto>>(null, "List is empty");
            }

            var response = new List<PayrollTransactionDto>();

            foreach (var i in payrollTransactions)
            {
                response.Add(MapToValue(i));
            }
            return new Response<List<PayrollTransactionDto>>(response.OrderBy(i => i.Employee).ToList(), "Returning List");
        }
        public Response<DataTable> GetPayrollDetailReport(PayrollDetailFilter filter)
        {
            DataTable dataTable = PayrollItemDetailReport(filter);
            if (dataTable == null)
            {
                return new Response<DataTable>(null, "List is empty");
            }
            return new Response<DataTable>(dataTable, "Returning List");
        }
        public async Task<Response<DataTable>> GetPayrollReport(DeptFilter data)
        {
            DataTable dataTable =await PayrollItemReport(data);
            if (dataTable == null)
            {
                return new Response<DataTable>(null, "List is empty");
            }
            return new Response<DataTable>(dataTable, "Returning List");
        }
        public async Task<MemoryStream> ExportPayrollDetailedReport(PayrollDetailFilter filter)
        {
            DataTable dataTable = PayrollItemDetailReport(filter);
            if (dataTable == null)
            {
                return null;
            }
            var stream = new MemoryStream();
            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromDataTable(dataTable, PrintHeaders: true);
                // Setting the properties
                for (var col = 5; col < dataTable.Columns.Count + 1; col++)
                {
                    workSheet.Column(col).Style.Numberformat.Format = "0.00";//apply the number formatting you need
                }

                //Make all text fit the cells
                workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();

                // of the first row
                workSheet.Row(1).Height = 20;
                workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Row(1).Style.Font.Bold = true;
                package.Save();
            }
            stream.Position = 0;
            return stream;
        }
        public Response<List<PayrollCampusTransactionDto>> GetPayrollCampusGroupReport(PayrollCampusReportFilter filter)
        {
            var months = new List<int?>();
            var years = new List<int?>();
            var campusIds = new List<int?>();
            var documentStatus = new List<DocumentStatus?>();

            if (filter.Month != null)
            {
                months.Add(filter.Month);
            }
            if (filter.Year != null)
            {
                years.Add(filter.Year);
            }

            if (filter.DocumentStatus != null)
            {
                documentStatus.Add(filter.DocumentStatus);
            }
            if (filter.CampusId != null)
            {
                campusIds.Add(filter.CampusId);
            }

            var payrollTransactions = _unitOfWork.PayrollTransaction
                .Find(new PayrollTransactionSpecs(months, years, campusIds, documentStatus))
                .ToList();


            if (payrollTransactions.Count() == 0)
            {
                return new Response<List<PayrollCampusTransactionDto>>(null, "List is empty");
            }



            var GroupByCampus = payrollTransactions.GroupBy(x => new { x.CampusId, x.Campus.Name, x.Month, x.Year, x.StatusId, x.Status.Status, x.Status.State });


            List<PayrollCampusTransactionDto> payrollCampusTransactionDto = new List<PayrollCampusTransactionDto>();
            foreach (var item in GroupByCampus)
            {
                payrollCampusTransactionDto.Add(new PayrollCampusTransactionDto
                {


                    CampusId = item.Key.CampusId,
                    Campus = item.Key.Name,
                    Month = item.Key.Month,
                    Year = item.Key.Year,
                    AdvancesAndDeductions = item.Sum(x => x.PayrollTransactionLines.Where(p => p.PayrollType == PayrollType.Deduction)
                                     .Sum(e => e.Amount)),
                    IncomeTax = item.Sum(x => x.PayrollTransactionLines.Where(p => p.PayrollType == PayrollType.TaxDeduction)
                                     .Sum(e => e.Amount)),
                    GrossSalary = item.Sum(x => x.GrossSalary),
                    NetAmount = item.Sum(x => x.NetSalary),
                    Status = item.Key.Status,
                    State = item.Key.State

                });
            }



            return new Response<List<PayrollCampusTransactionDto>>(payrollCampusTransactionDto, "Returning List");

        }
        public Response<PayrollExecutiveReportDto> GetPayrollExecutiveReport(PayrollExecutiveReportFilter filter)
        {
            var months = new List<int?>();
            var campuses = new List<int?>();

            if (filter.Month == null)
            {
                filter.Month = new int?[0];
            }

            if (filter.CampusId != null)
            {
                campuses.Add(filter.CampusId);
            }

            //Fetching payroll as per the filters
            var getPayrollTransaction = _unitOfWork.PayrollTransaction.Find(new PayrollTransactionSpecs(filter.Month,
                (int)filter.Year, campuses)).ToList();

            //if (getPayrollTransaction.Count() == 0)
            //    return new Response<PayrollExecutiveReportDto>("Payroll not found");

            //Selecting all payroll Items grouped by their payrollItem
            var itemList = new List<PayrollItemsDto>();

            foreach (var payroll in getPayrollTransaction)
            {
                //Adding basic payItems in PayrollItemsDto

                itemList.Add(new PayrollItemsDto()
                {
                    AccountId = payroll.BPSAccountId,
                    AccountName = payroll.BasicPayItem.Account.Name,
                    PayrollType = PayrollType.BasicPay,
                    Amount = payroll.BasicSalary,
                });

                //filtering other payrollItems (Allowance, deduction,assignment allowance)
                var payrollItems = payroll.PayrollTransactionLines
                    .Where(e => (
                    filter.AccountId == null ? true :
                    filter.AccountId != null ? (e.AccountId == filter.AccountId) : false))
                    .ToList();

                if (payrollItems.Count() > 0)
                {
                    foreach (var lines in payrollItems)
                    {
                        itemList.Add(new PayrollItemsDto()
                        {
                            AccountId = lines.AccountId,
                            AccountName = lines.Account.Name,
                            PayrollType = lines.PayrollType,
                            Amount = lines.Amount
                        });
                    }
                }
            }

            itemList = itemList.Where(e => (
                    filter.AccountId == null ? true :
                    filter.AccountId != null ? (e.AccountId == filter.AccountId) : false))
                .GroupBy(x => new { x.PayrollType, x.AccountId, x.AccountName })
                .Select(c => new PayrollItemsDto
                {
                    AccountId = c.Key.AccountId,
                    AccountName = c.Key.AccountName,
                    PayrollType = c.Key.PayrollType,
                    Amount = c.Sum(e => e.Amount)
                })
                .ToList();

            //calculating payrollAmount by their employeeType
            var sumTotalOfEmployeeType = getPayrollTransaction
                .GroupBy(i => i.EmployeeType)
                .Select(i => new
                {
                    EmployeeType = i.Key,
                    Amount = i.Sum(s => s.NetSalary),
                }).ToList();

            var result = new PayrollExecutiveReportDto()
            {
                ContractualAmount = sumTotalOfEmployeeType
                        .Where(i => i.EmployeeType == "Contract")
                        .Select(i => i.Amount).FirstOrDefault(),
                RegularAmount = sumTotalOfEmployeeType
                        .Where(i => i.EmployeeType == "Regular")
                        .Select(i => i.Amount).FirstOrDefault(),
                TenureAmount = sumTotalOfEmployeeType
                        .Where(i => i.EmployeeType == "Tenure")
                        .Select(i => i.Amount).FirstOrDefault(),
                PayrollItems = itemList
            };

            return new Response<PayrollExecutiveReportDto>(result, "Payroll found");
        }
        public Response<List<BankAdviceReportDto>> GetBankAdviceReportReport(BankAdviceReportFilter filter)
        {
            var campuses = new List<int?>();

            if (filter.CampusId != null)
            {
                campuses.Add(filter.CampusId);
            }

            //Fetching payroll as per the filters
            var payrollTransactions = _unitOfWork.PayrollTransaction.Find(new PayrollTransactionSpecs((int)filter.Month, (int)filter.Year, campuses)).ToList();

            if (payrollTransactions.Count() == 0)
            {
                return new Response<List<BankAdviceReportDto>>(null, "List is empty");
            }

            var response = new List<BankAdviceReportDto>();

            foreach (var payroll in payrollTransactions)
            {
                response.Add(new BankAdviceReportDto()
                {
                    EmployeeName = payroll.Name,
                    BankName = payroll.BankName,
                    BranchName = payroll.BranchName,
                    AccountNumber = payroll.Employee.AccountNumber,
                    Amount = payroll.NetSalary
                });
            }

            return new Response<List<BankAdviceReportDto>>(response, "Returning Bank advice report");
        }

        private async Task<DataTable> PayrollItemReport(DeptFilter data)
        {          

            var payrollTransactions = _unitOfWork.PayrollTransaction
                .Find(new PayrollTransactionSpecs((int)data.Month, (int)data.Year, data.DepartmentId, (int)data.CampusId))
                .ToList();

            if (payrollTransactions.Count() > 0)
            {
                foreach (var payrollTransaction in payrollTransactions)
                {
                    var payroll = new UpdatePayrollTransactionDto()
                    {
                        Id = payrollTransaction.Id,
                        AccountPayableId = data.AccountPayableId,
                        isSubmit = false,
                    };

                    var result = await this.UpdatePayrollTransaction(payroll, payrollTransaction.StatusId);
                   
                }
            }

            var getpayrollTransactions = _unitOfWork.PayrollTransaction.Find(new PayrollTransactionSpecs((int)data.Month, (int)data.Year, data.DepartmentId, (int)data.CampusId, true)).ToList();

            if (getpayrollTransactions.Count() == 0)
            {
                return null;
            }

            var allowanceDTO = new List<PayrollTransactionDto>();


            foreach (var payroll in payrollTransactions)
            {


                if (payroll.PayrollTransactionLines.Count() > 0)
                {
                    foreach (var lines in payroll.PayrollTransactionLines)
                    {
                        allowanceDTO.Add(new PayrollTransactionDto
                        {
                            Id = payroll.Id,
                            EmployeeId = payroll.EmployeeId,
                            Employee = payroll.Employee.Name,
                            CNIC = payroll.CNIC,
                            Department = payroll.Department.Name,
                            Campus = payroll.Campus.Name,
                            Designation = payroll.Designation.Name,
                            AccountName = lines.Account.Name,
                            Amount = lines.Amount,
                            NetSalary = payroll.NetSalary,
                            GrossPay = payroll.GrossSalary
                            
                        });
                    }
                }
            }

            allowanceDTO = allowanceDTO
               .GroupBy(x => new { x.Employee, x.NetSalary, x.GrossPay, x.CNIC, x.Department, x.Campus, x.Designation, x.AccountName, x.EmployeeId, x.Id })
               .Select(c => new PayrollTransactionDto
               {
                   Id = c.Key.Id,
                   EmployeeId = c.Key.EmployeeId,
                   Employee = c.Key.Employee,
                   CNIC = c.Key.CNIC,
                   Department = c.Key.Department,
                   Campus = c.Key.Campus,
                   Designation = c.Key.Designation,
                   AccountName = c.Key.AccountName,
                   Amount = c.Sum(e => e.Amount),
                   NetSalary = c.Key.NetSalary,
                   GrossPay = c.Key.GrossPay
               })
               .ToList();

            var groups = from d in allowanceDTO
                         group d by new { d.Employee, d.NetSalary, d.GrossPay, d.CNIC, d.Department, d.Campus, d.Designation ,d.EmployeeId,d.Id}
                        into grp
                         select new
                         {    
                             Id = grp.Key.Id,
                             EmployeeId = grp.Key.EmployeeId,
                             Employee = grp.Key.Employee,
                             CNIC = grp.Key.CNIC,
                             Department = grp.Key.Department,
                             Campus = grp.Key.Campus,
                             Designation = grp.Key.Designation,
                             NetSalary = grp.Key.NetSalary,
                             GrossPay = grp.Key.GrossPay,
                             Items = grp.Select(d2 => new { d2.AccountName, d2.Amount }).ToArray()
                         };

            /*get all possible subjects into a separate group*/
            var itemNames = (from d in allowanceDTO
                             select d.AccountName).Distinct();


            DataTable dt = new DataTable();
            /*for static cols*/

            dt.Columns.Add("Id");
            dt.Columns.Add("EmployeeId");
            dt.Columns.Add("Employee");
            dt.Columns.Add("CNIC");
            dt.Columns.Add("NetSalary");
            dt.Columns.Add("GrossPay");
            dt.Columns.Add("Department");
            dt.Columns.Add("Campus");
            dt.Columns.Add("Designation");
            /*for dynamic cols*/
            foreach (var item in itemNames)
            {
                dt.Columns.Add(item.ToString());
            }

            /*pivot the data into a new datatable*/
            foreach (var g in groups)
            {
                DataRow dr = dt.NewRow();
                dr["Id"] = g.Id;
                dr["EmployeeId"] = g.EmployeeId;
                dr["Employee"] = g.Employee;
                dr["CNIC"] = g.CNIC;
                dr["Department"] = g.Department;
                dr["NetSalary"] = g.NetSalary;
                dr["GrossPay"] = g.GrossPay;
                dr["Campus"] = g.Campus;
                dr["Designation"] = g.Designation;

                foreach (var item in g.Items)
                {
                    dr[item.AccountName] = item.Amount;
                }
                dt.Rows.Add(dr);
            }
            return dt;

        }
    }
}
