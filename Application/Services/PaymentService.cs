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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEmployeeService _employeeService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PaymentService(IUnitOfWork unitOfWork, IMapper mapper, IEmployeeService employeeService, IHttpContextAccessor httpContextAccessor, IPayrollTransactionService payrollTransactionService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _employeeService = employeeService;
        }


        public async Task<Response<PaymentDto>> CreateAsync(CreatePaymentDto entity)
        {
            if (entity.isSubmit)
            {
                return await this.SubmitPay(entity);
            }
            else
            {
                return await this.SavePay(entity, 1);
            }
        }

        public async Task<PaginationResponse<List<PaymentDto>>> GetAllAsync(PaginationFilter filter, DocType docType)
        {
            var specification = new PaymentSpecs(filter, docType);
            var payment = await _unitOfWork.Payment.GetAll(specification);

            if (payment.Count() == 0)
                return new PaginationResponse<List<PaymentDto>>(_mapper.Map<List<PaymentDto>>(payment), "List is empty");

            var totalRecords = await _unitOfWork.Payment.TotalRecord(new PaymentSpecs(docType));

            return new PaginationResponse<List<PaymentDto>>(_mapper.Map<List<PaymentDto>>(payment), filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<PaymentDto>> GetByIdAsync(int id, DocType docType)
        {
            var specification = new PaymentSpecs(false, docType);
            var payment = await _unitOfWork.Payment.GetById(id, specification);
            if (payment == null)
                return new Response<PaymentDto>("Not found");

            var paymentDto = _mapper.Map<PaymentDto>(payment);

            paymentDto.IsAllowedRole = false;
            var workflow = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(docType)).FirstOrDefault();


            if (workflow != null)
            {
                var transition = workflow.WorkflowTransitions
                    .FirstOrDefault(x => (x.CurrentStatusId == paymentDto.StatusId));

                if (transition != null)
                {
                    var currentUserRoles = new GetUser(this._httpContextAccessor).GetCurrentUserRoles();
                    foreach (var role in currentUserRoles)
                    {
                        if (transition.AllowedRole.Name == role)
                        {
                            paymentDto.IsAllowedRole = true;
                        }
                    }
                }
            }
            return new Response<PaymentDto>(paymentDto, "Returning value");
        }

        public async Task<Response<PaymentDto>> UpdateAsync(CreatePaymentDto entity)
        {
            if (entity.isSubmit)
            {
                return await this.SubmitPay(entity);
            }
            else
            {
                return await this.UpdatePay(entity, 1);
            }
        }

        private async Task<Response<PaymentDto>> SubmitPay(CreatePaymentDto entity)
        {
            var checkingActiveWorkFlows = _unitOfWork.WorkFlow.Find(
                new WorkFlowSpecs(entity.PaymentFormType)).FirstOrDefault();

            if (checkingActiveWorkFlows == null)
            {
                return new Response<PaymentDto>("No workflow found for this document");
            }

            if (entity.Id == null)
            {
                return await this.SavePay(entity, 6);
            }
            else
            {
                return await this.UpdatePay(entity, 6);
            }
        }

        private async Task<Response<PaymentDto>> SavePay(CreatePaymentDto entity, int status)
        {
            var payment = _mapper.Map<Payment>(entity);

            //Setting status
            payment.setStatus(status);

            _unitOfWork.CreateTransaction();
            try
            {
                //Saving in table
                var result = await _unitOfWork.Payment.Add(payment);
                await _unitOfWork.SaveAsync();

                //For creating docNo
                payment.CreateDocNo();
                await _unitOfWork.SaveAsync();

                //Commiting the transaction 
                _unitOfWork.Commit();

                //returning response
                return new Response<PaymentDto>(_mapper.Map<PaymentDto>(result), "Created successfully");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new Response<PaymentDto>(ex.Message);
            }
        }

        private async Task<Response<PaymentDto>> UpdatePay(CreatePaymentDto entity, int status)
        {
            var specification = new PaymentSpecs(true, entity.PaymentFormType);
            var payment = await _unitOfWork.Payment.GetById((int)entity.Id, specification);

            if (payment == null)
                return new Response<PaymentDto>("Not found");

            if (payment.StatusId != 1 && payment.StatusId != 2)
                return new Response<PaymentDto>("Only draft payments can be edited");

            payment.setStatus(status);

            _unitOfWork.CreateTransaction();
            try
            {
                //For updating data
                _mapper.Map<CreatePaymentDto, Payment>(entity, payment);

                //saving into database
                await _unitOfWork.SaveAsync();

                //Commiting the transaction
                _unitOfWork.Commit();

                //returning response
                return new Response<PaymentDto>(_mapper.Map<PaymentDto>(payment), "Updated successfully");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new Response<PaymentDto>(ex.Message);
            }
        }

        private async Task AddToLedger(Payment payment)
        {
            var transaction = new Transactions(payment.DocNo, payment.PaymentFormType);
            await _unitOfWork.Transaction.Add(transaction);
            await _unitOfWork.SaveAsync();

            payment.setTransactionId(transaction.Id);
            await _unitOfWork.SaveAsync();

            if (payment.PaymentType == PaymentType.Inflow)
            {
                var addGrossAmountInRecordLedger = new RecordLedger(
                    transaction.Id,
                    payment.AccountId,
                    payment.BusinessPartnerId,
                    null,
                    payment.Description,
                    'C',
                    payment.GrossPayment,
                    payment.CampusId,
                    payment.PaymentDate
                    );

                await _unitOfWork.Ledger.Add(addGrossAmountInRecordLedger);

                if (payment.Discount > 0)
                {
                    var addDiscountInRecordLedger = new RecordLedger(
                        transaction.Id,
                    payment.AccountId,
                    payment.BusinessPartnerId,
                    null,
                    payment.Description,
                    'D',
                    payment.Discount,
                    payment.CampusId,
                    payment.PaymentDate
                        );

                    await _unitOfWork.Ledger.Add(addDiscountInRecordLedger);
                }

                if (payment.SRBTax > 0)
                {
                    var addSRBInRecordLedger = new RecordLedger(
                        transaction.Id,
                    payment.AccountId,
                    payment.BusinessPartnerId,
                    null,
                    payment.Description,
                    'D',
                    payment.SRBTax,
                    payment.CampusId,
                    payment.PaymentDate
                        );

                    await _unitOfWork.Ledger.Add(addSRBInRecordLedger);
                }

                if (payment.SalesTax > 0)
                {
                    var addSalesTaxInRecordLedger = new RecordLedger(

                    transaction.Id,
                    payment.AccountId,
                    payment.BusinessPartnerId,
                    null,
                    payment.Description,
                    'D',
                    payment.SalesTax,
                    payment.CampusId,
                    payment.PaymentDate
                        );
                    await _unitOfWork.Ledger.Add(addSalesTaxInRecordLedger);
                }

                if (payment.IncomeTax > 0)
                {
                    var addIncomeTaxInRecordLedger = new RecordLedger(

                    transaction.Id,
                    payment.AccountId,
                    payment.BusinessPartnerId,
                    null,
                    payment.Description,
                    'D',
                    payment.IncomeTax,
                    payment.CampusId,
                    payment.PaymentDate);
                    await _unitOfWork.Ledger.Add(addIncomeTaxInRecordLedger);
                }
                var addNetPaymentInRecordLedger = new RecordLedger(
                    transaction.Id,
                    payment.PaymentRegisterId,
                    payment.BusinessPartnerId,
                    null,
                    payment.Description,
                    'D',
                    (payment.GrossPayment - payment.Discount - payment.SalesTax - payment.IncomeTax - payment.SRBTax),
                    payment.CampusId,
                    payment.PaymentDate);

                await _unitOfWork.Ledger.Add(addNetPaymentInRecordLedger);
            }

            if (payment.PaymentType == PaymentType.Outflow)
            {
                var addGrossAmountInRecordLedger = new RecordLedger(
                    transaction.Id,
                    payment.AccountId,
                    payment.BusinessPartnerId,
                    null,
                    payment.Description,
                    'D',
                    payment.GrossPayment,
                    payment.CampusId,
                    payment.PaymentDate
                    );

                await _unitOfWork.Ledger.Add(addGrossAmountInRecordLedger);

                if (payment.Discount > 0)
                {
                    var addDiscountInRecordLedger = new RecordLedger(
                    transaction.Id,
                    payment.AccountId,
                    payment.BusinessPartnerId,
                    null,
                    payment.Description,
                    'C',
                    payment.Discount,
                    payment.CampusId,
                    payment.PaymentDate
                        );

                    await _unitOfWork.Ledger.Add(addDiscountInRecordLedger);
                }

                if (payment.SRBTax > 0)
                {
                    var addSRBInRecordLedger = new RecordLedger(
                     transaction.Id,
                    payment.AccountId,
                    payment.BusinessPartnerId,
                    null,
                    payment.Description,
                    'C',
                    payment.SRBTax,
                    payment.CampusId,
                    payment.PaymentDate
                        );

                    await _unitOfWork.Ledger.Add(addSRBInRecordLedger);
                }

                if (payment.SalesTax > 0)
                {
                    var addSalesTaxInRecordLedger = new RecordLedger(

                    transaction.Id,
                    payment.AccountId,
                    payment.BusinessPartnerId,
                    null,
                    payment.Description,
                    'C',
                    payment.SalesTax,
                    payment.CampusId,
                    payment.PaymentDate
                        );
                    await _unitOfWork.Ledger.Add(addSalesTaxInRecordLedger);
                }

                if (payment.IncomeTax > 0)
                {
                    var addIncomeTaxInRecordLedger = new RecordLedger(

                    transaction.Id,
                    payment.AccountId,
                    payment.BusinessPartnerId,
                    null,
                    payment.Description,
                    'C',
                    payment.IncomeTax,
                    payment.CampusId,
                    payment.PaymentDate);
                    await _unitOfWork.Ledger.Add(addIncomeTaxInRecordLedger);
                }
                var addNetPaymentInRecordLedger = new RecordLedger(
                    transaction.Id,
                    payment.PaymentRegisterId,
                    payment.BusinessPartnerId,
                    null,
                    payment.Description,
                    'C',
                    (payment.GrossPayment - payment.Discount - payment.SalesTax - payment.IncomeTax - payment.SRBTax),
                    payment.CampusId,
                    payment.PaymentDate);

                await _unitOfWork.Ledger.Add(addNetPaymentInRecordLedger);
            }

            await _unitOfWork.SaveAsync();
        }

        public async Task<Response<bool>> CheckWorkFlow(ApprovalDto data)
        {
            var getPayment = await _unitOfWork.Payment.GetById(data.DocId, new PaymentSpecs());

            if (getPayment == null)
            {
                return new Response<bool>("Payment with the input id not found");
            }
            if (getPayment.Status.State == DocumentStatus.Unpaid || getPayment.Status.State == DocumentStatus.Partial || getPayment.Status.State == DocumentStatus.Paid)
            {
                return new Response<bool>("Payment already approved");
            }
            var workflow = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(getPayment.PaymentFormType)).FirstOrDefault();

            if (workflow == null)
            {
                return new Response<bool>("No activated workflow found for this document");
            }
            var transition = workflow.WorkflowTransitions
                    .FirstOrDefault(x => (x.CurrentStatusId == getPayment.StatusId && x.Action == data.Action));

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
                        getPayment.setStatus(transition.NextStatusId);
                        if (transition.NextStatus.State == DocumentStatus.Unpaid)
                        {
                            getPayment.setReconStatus(DocumentStatus.Unreconciled);
                            await AddToLedger(getPayment);
                            _unitOfWork.Commit();
                            return new Response<bool>(true, "Payment Approved");
                        }
                        if (transition.NextStatus.State == DocumentStatus.Rejected)
                        {
                            await _unitOfWork.SaveAsync();
                            _unitOfWork.Commit();
                            return new Response<bool>(true, "Payment Rejected");
                        }
                        await _unitOfWork.SaveAsync();
                        _unitOfWork.Commit();
                        return new Response<bool>(true, "Payment Reviewed");
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

        //Overrided method
        public Task<PaginationResponse<List<PaymentDto>>> GetAllAsync(PaginationFilter filter)
        {
            throw new NotImplementedException();
        }
        //Overrided method
        public Task<Response<PaymentDto>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Response<List<UnReconStmtDto>> GetBankUnreconciledPayments(Guid id)
        {
            List<UnReconStmtDto> unreconciledBankPaymentsStatus = new List<UnReconStmtDto>();

            var getBankReconStatus = _unitOfWork.Payment.Find(new PaymentSpecs(id)).ToList();

            if (getBankReconStatus.Count == 0)
                 return new Response<List<UnReconStmtDto>>(unreconciledBankPaymentsStatus, "Unreconciled bank statements not found");

            foreach (var e in getBankReconStatus)
            {
                var netPayment = e.GrossPayment - e.Discount - e.IncomeTax - e.SalesTax - e.SRBTax;
                var reconciledPayment = _unitOfWork.BankReconciliation.Find(new BankReconSpecs(e.Id, true)).Sum(a => a.Amount);


                var mapingValueInDTO = new UnReconStmtDto
                {
                    Id = e.Id,
                    DocNo = e.DocNo,
                    DocDate = e.PaymentDate,
                    Amount = netPayment,
                    Description = e.Description,
                    ReconciledAmount = reconciledPayment,
                    UnreconciledAmount = e.PaymentType == PaymentType.Inflow ? (netPayment - reconciledPayment) : ((netPayment - reconciledPayment) * -1),
                    BankReconStatus = (DocumentStatus)e.BankReconStatus
                };
                unreconciledBankPaymentsStatus.Add(mapingValueInDTO);
            };

            return new Response<List<UnReconStmtDto>>(unreconciledBankPaymentsStatus, "Returning bank unreconciled payments");

        }

        public async Task<Response<PaymentDto>> CreatePayrollPaymentProcess(CreatePayrollPaymentDto data)
        {
            foreach (var line in data.CreatePayrollTransLines)
            {
                var bp = await _unitOfWork.BusinessPartner.GetById(line.BusinessPartnerId);
                if (bp != null)
                {
                    if (bp.BusinessPartnerType != BusinessPartnerType.Employee)
                        return new Response<PaymentDto>("Business Partner is not employee");
                }
                var payment = new CreatePaymentDto()
                {
                    PaymentType = PaymentType.Outflow,
                    PaymentFormType = DocType.PayrollPayment,
                    BusinessPartnerId = line.BusinessPartnerId,
                    AccountId = line.AccountPayableId,
                    CampusId = data.CampusId,
                    GrossPayment = line.NetSalary,
                    PaymentDate = data.PaymentDate,
                    PaymentRegisterType = data.PaymentRegisterType,
                    PaymentRegisterId = data.PaymentRegisterId,
                    Description = data.Description,
                    Discount = 0,
                    SalesTax = 0,
                    IncomeTax = 0,
                    DocumentTransactionId = line.TransactionId,
                    isSubmit = false
                };

                if (payment.isSubmit)
                {
                    var result = await this.SubmitPay(payment);
                    if (!result.IsSuccess)
                    {
                        return new Response<PaymentDto>(""); ;
                    }
                }
                else
                {
                    var result = await this.SavePay(payment, 1);
                    if (!result.IsSuccess)
                    {
                        return new Response<PaymentDto>(""); ;
                    }
                }
            }

            return new Response<PaymentDto>(null, "Payroll Payment created successfully");
        }

        public Response<List<PayrollTransactionDto>> GetPayrollTransactionByDept(DeptFilter data)
        {
            var payrollTransactions = _unitOfWork.PayrollTransaction.Find(new PayrollTransactionSpecs(data.Month, data.Year, data.DepartmentId, false)).ToList();

            if (payrollTransactions.Count == 0)
                return new Response<List<PayrollTransactionDto>>("list is empty");

            var response = new List<PayrollTransactionDto>();

            foreach (var i in payrollTransactions)
            {
                response.Add(new PayrollTransactionService(_unitOfWork, _mapper, _employeeService, _httpContextAccessor).MapToValue(i));
            }
            var result = response.OrderBy(i => i.Employee).ToList();

            return new Response<List<PayrollTransactionDto>>(result, "Returning Payroll Transactions");
        }

        public Response<List<PaymentDto>> GetPaymentByDept(DeptFilter data)
        {
            var payrollTransactions = _unitOfWork.PayrollTransaction.Find(new PayrollTransactionSpecs(data.Month, data.Year, data.DepartmentId, "")).ToList();

            if (payrollTransactions.Count == 0)
                return new Response<List<PaymentDto>>("list is empty");

            var getPayments = _unitOfWork.Payment.Find(new PaymentSpecs(DocType.PayrollPayment, false)).ToList();

            var response = new List<PaymentDto>();

            foreach (var t in payrollTransactions)
            {
                var payment = getPayments.FirstOrDefault(x => x.TransactionId == t.TransactionId);
                var paymentDto = _mapper.Map<PaymentDto>(payment);

                if (payment != null)
                {
                    response.Add(paymentDto);
                }
            }
            var result = response.OrderBy(x => x.BusinessPartnerId).ToList();

            return new Response<List<PaymentDto>>(result, "Returning payroll payments");

        }

        public async Task<Response<bool>> ProcessForEditPayrollPayment(int[] id)
        {
            _unitOfWork.CreateTransaction();
            try
            {
                var checkingActiveWorkFlows = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.PayrollPayment)).FirstOrDefault();

                if (checkingActiveWorkFlows == null)
                {
                    return new Response<bool>("No workflow found for Payroll payment");
                }

                for (int i = 0; i < id.Length; i++)
                {
                    var getPayment = await _unitOfWork.Payment.GetById(id[i], new PayrollPaymentSpecs());

                    if (getPayment == null)
                        return new Response<bool>($"Payroll Payment with the id = {id[i]} not found");

                    if (getPayment.BusinessPartner.BusinessPartnerType != BusinessPartnerType.Employee)
                        return new Response<bool>($"Payroll Payment with the id = {id[i]} business partner is not employee");

                    getPayment.setStatus(6);
                    await _unitOfWork.SaveAsync();
                }
                _unitOfWork.Commit();

                return new Response<bool>(true, "Payment submitted successfully");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new Response<bool>(ex.Message);
            }
        }

        public Response<bool> GetPaymentForApproval(DeptFilter data)
        {
            var payrollTransactions = _unitOfWork.PayrollTransaction.Find(new PayrollTransactionSpecs(data.Month, data.Year, data.DepartmentId, "")).ToList();

                if (payrollTransactions.Count == 0)
                return new Response<bool>("list is empty");

            var getPayments = _unitOfWork.Payment.Find(new PaymentSpecs( DocType.PayrollPayment, true)).ToList();
           
            var response = new List<PaymentDto>();

            foreach (var t in payrollTransactions)
            {
                var payment = getPayments.FirstOrDefault(x => x.TransactionId == t.TransactionId);
                var paymentDto = _mapper.Map<PaymentDto>(payment);

                if (payment != null)
                {
                    response.Add(paymentDto);
                }
            }

            var result = response.OrderBy(x => x.BusinessPartnerId).ToList();

            return new Response<bool>(true, "Returning payroll payments");
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
            return new Response<bool>(true, "Payroll payment approval process completed successfully");

        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

    }
}
