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
            if ((bool)entity.isSubmit)
            {
                return await this.SubmitPay(entity);
            }
            else
            {
                return await this.SavePay(entity, 1);
            }
        }

        public async Task<PaginationResponse<List<PaymentDto>>> GetAllAsync(TransactionFormFilter filter, DocType docType)
        {
            var docDate = new List<DateTime?>();
            var dueDate = new List<DateTime?>();
            var states = new List<DocumentStatus?>();

            if (filter.DocDate != null)
            {
                docDate.Add(filter.DocDate);
            }
            if (filter.DueDate != null)
            {
                dueDate.Add(filter.DueDate);
            }
            if (filter.State != null)
            {
                states.Add(filter.State);
            }
            var payment = await _unitOfWork.Payment.GetAll(new PaymentSpecs(docDate, dueDate, states, filter, docType, false));

            if (payment.Count() == 0)
                return new PaginationResponse<List<PaymentDto>>(_mapper.Map<List<PaymentDto>>(payment), "List is empty");

            var totalRecords = await _unitOfWork.Payment.TotalRecord(new PaymentSpecs(docDate, dueDate, states, filter, docType, true));

            return new PaginationResponse<List<PaymentDto>>(_mapper.Map<List<PaymentDto>>(payment), filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<PaymentDto>> GetByIdAsync(int id, DocType docType)
        {
            var specification = new PaymentSpecs(false, docType);
            var payment = await _unitOfWork.Payment.GetById(id, specification);
            if (payment == null)
                return new Response<PaymentDto>("Not found");

            var paymentDto = _mapper.Map<PaymentDto>(payment);

            var getBank = _unitOfWork.BankAccount.Find(new BankAccountSpecs(payment.PaymentRegisterId)).FirstOrDefault();


            // Mapping Bank details in payment 
            paymentDto.BankName = getBank.BankName;
            paymentDto.AccountTitle = getBank.AccountTitle;
            paymentDto.AccountNumber = getBank.AccountNumber;

            //Returning
            ReturningRemarks(paymentDto, docType);

            ReturningFiles(paymentDto, docType);

            var workflow = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(docType)).FirstOrDefault();
            if ((paymentDto.State == DocumentStatus.Unpaid || paymentDto.State == DocumentStatus.Partial || paymentDto.State == DocumentStatus.Paid) && paymentDto.TransactionId != null && paymentDto.LedgerId != null)
            {
                return new Response<PaymentDto>(MapToValue(paymentDto), "Returning value");
            }

            paymentDto.IsAllowedRole = false;
            if (payment.DocumentLedgerId != null)
            {
                var ledger = _unitOfWork.Ledger.Find(new LedgerSpecs((int)payment.DocumentLedgerId, false)).FirstOrDefault();
                if (ledger != null)
                {
                    paymentDto.DocumentReconcile = new PaidDocListDto
                    {
                        Id = ledger.Transactions.DocId,
                        DocNo = ledger.Transactions.DocNo,
                        DocType = ledger.Transactions.DocType,
                        Amount = ledger.Amount
                    };
                }
            }

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
            if ((bool)entity.isSubmit)
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

            if (entity.Deduction > 0)
            {
                if (entity.DeductionAccountId == null)
                    return new Response<PaymentDto>("Deduction account is required");

                var AccountIdlevel4 = await _unitOfWork.Level4.GetById((Guid)entity.DeductionAccountId);

                var AccountIdlevel3 = ReceivableAndPayable.Validate(AccountIdlevel4.Level3_id);

                if (AccountIdlevel3 == false)
                {
                    return new Response<PaymentDto>("Account Invalid");
                }
            }

            //Validation for same  Accounts
            if (entity.PaymentRegisterId == entity.AccountId || entity.PaymentRegisterId == entity.DeductionAccountId || entity.DeductionAccountId == entity.AccountId)
            {
                return new Response<PaymentDto>("Accounts Cannot Be Same");
            }

            //Validation for Payable and Receivable
            var BankAccountlevel4 = await _unitOfWork.Level4.GetById((Guid)entity.PaymentRegisterId);

            var BankAccount = ReceivableAndPayable.ValidateBankAccount( BankAccountlevel4.Level3_id);

            if (BankAccount == false )
            {
                return new Response<PaymentDto>("Payment register account Invalid");
            }

            //Setting status
            payment.setStatus(status);

            _unitOfWork.CreateTransaction();
          
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

        private async Task<Response<PaymentDto>> UpdatePay(CreatePaymentDto entity, int status)
        {
            var specification = new PaymentSpecs(true, entity.PaymentFormType);
            var payment = await _unitOfWork.Payment.GetById((int)entity.Id, specification);
            
            if (entity.Deduction > 0)
            {
                if (entity.DeductionAccountId == null)
                    return new Response<PaymentDto>("Deduction account is required");

                var AccountIdlevel4 = await _unitOfWork.Level4.GetById((Guid)entity.DeductionAccountId);

                var AccountIdlevel3 = ReceivableAndPayable.Validate(AccountIdlevel4.Level3_id);

                if (AccountIdlevel3 == false)
                {
                    return new Response<PaymentDto>("Account Invalid");
                }
            }

            if (payment == null)
                return new Response<PaymentDto>("Not found");

            if (payment.StatusId != 1 && payment.StatusId != 2)
                return new Response<PaymentDto>("Only draft payments can be edited");
            //Validation for same  Accounts
            if (entity.PaymentRegisterId == entity.AccountId || entity.PaymentRegisterId == entity.DeductionAccountId || entity.DeductionAccountId == entity.AccountId)
            {
                return new Response<PaymentDto>("Accounts Cannot Be Same");
            }

            //Validation for Payable and Receivable
            var BankAccountlevel4 = await _unitOfWork.Level4.GetById((Guid)entity.PaymentRegisterId);

            var BankAccount = ReceivableAndPayable.ValidateBankAccount(BankAccountlevel4.Level3_id);

            if (BankAccount == false)
            {
                return new Response<PaymentDto>("Payment register account Invalid");
            }

            //var level4 = await _unitOfWork.Level4.GetById((Guid)entity.AccountId);

            //var level3 = ReceivableAndPayable.Validate(level4.Level3_id);

            //if (level3 == false)
            //{
            //    return new Response<PaymentDto>("Deduction account Invalid");
            //}

            payment.setStatus(status);

            if (payment.DocumentLedgerId != null)
                entity.DocumentLedgerId = payment.DocumentLedgerId;

            _unitOfWork.CreateTransaction();
            
                //For updating data
                _mapper.Map<CreatePaymentDto, Payment>(entity, payment);

                //saving into database
                await _unitOfWork.SaveAsync();

                //Commiting the transaction
                _unitOfWork.Commit();

                //returning response
                return new Response<PaymentDto>(_mapper.Map<PaymentDto>(payment), "Updated successfully");
        
        }

        private async Task<Response<bool>> AddToLedger(Payment payment)
        {
            var sRBTax = (payment.GrossPayment * payment.SRBTax) / 100;
            var incomeTax = (payment.GrossPayment * payment.IncomeTax) / 100;
            var salesTax = (payment.GrossPayment * payment.SalesTax) / 100;

            var transaction = new Transactions(payment.Id, payment.DocNo, payment.PaymentFormType);
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


                if (payment.SRBTax > 0)
                {
                    var getTaxAccount = _unitOfWork.Taxes.Find(new TaxesSpecs(TaxType.SRBTaxAsset)).Select(i => i.AccountId).FirstOrDefault();
                    if (getTaxAccount == null)
                        return new Response<bool>("SRB Tax Account not found");

                    var addSRBInRecordLedger = new RecordLedger(
                    transaction.Id,
                    (Guid)getTaxAccount,
                    payment.BusinessPartnerId,
                    null,
                    payment.Description,
                    'D',
                    sRBTax,
                    payment.CampusId,
                    payment.PaymentDate
                        );

                    await _unitOfWork.Ledger.Add(addSRBInRecordLedger);
                }

                if (payment.SalesTax > 0)
                {
                    var getTaxAccount = _unitOfWork.Taxes.Find(new TaxesSpecs(TaxType.SalesTaxAsset)).Select(i => i.AccountId).FirstOrDefault();
                    if (getTaxAccount == null)
                        return new Response<bool>("Sales Tax Account not found");

                    var addSalesTaxInRecordLedger = new RecordLedger(

                    transaction.Id,
                    (Guid)getTaxAccount,
                    payment.BusinessPartnerId,
                    null,
                    payment.Description,
                    'D',
                    salesTax,
                    payment.CampusId,
                    payment.PaymentDate
                        );
                    await _unitOfWork.Ledger.Add(addSalesTaxInRecordLedger);
                }

                if (payment.IncomeTax > 0)
                {
                    var getTaxAccount = _unitOfWork.Taxes.Find(new TaxesSpecs(TaxType.IncomeTaxAsset)).Select(i => i.AccountId).FirstOrDefault();
                    if (getTaxAccount == null)
                        return new Response<bool>("Sales Tax Account not found");

                    var addIncomeTaxInRecordLedger = new RecordLedger(

                    transaction.Id,
                    (Guid)getTaxAccount,
                    payment.BusinessPartnerId,
                    null,
                    payment.Description,
                    'D',
                    incomeTax,
                    payment.CampusId,
                    payment.PaymentDate);
                    await _unitOfWork.Ledger.Add(addIncomeTaxInRecordLedger);
                }

                if (payment.Deduction > 0)
                {
                    if (payment.DeductionAccountId == null)
                        return new Response<bool>("Deduction Account not found");

                    var addDeductionRecordLedger = new RecordLedger(

                    transaction.Id,
                    (Guid)payment.DeductionAccountId,
                    payment.BusinessPartnerId,
                    null,
                    payment.Description,
                    'D',
                    payment.Deduction,
                    payment.CampusId,
                    payment.PaymentDate);
                    await _unitOfWork.Ledger.Add(addDeductionRecordLedger);
                }

                var addNetPaymentInRecordLedger = new RecordLedger(
                    transaction.Id,
                    payment.PaymentRegisterId,
                    payment.BusinessPartnerId,
                    null,
                    payment.Description,
                    'D',
                    (payment.GrossPayment - salesTax - incomeTax - sRBTax - payment.Deduction),
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

                if (payment.SRBTax > 0)
                {
                    var getTaxAccount = _unitOfWork.Taxes.Find(new TaxesSpecs(TaxType.SRBTaxLiability)).Select(i => i.AccountId).FirstOrDefault();
                    if (getTaxAccount == null)
                        return new Response<bool>("SRB Tax Account not found");

                    var addSRBInRecordLedger = new RecordLedger(
                    transaction.Id,
                    (Guid)getTaxAccount,
                    payment.BusinessPartnerId,
                    null,
                    payment.Description,
                    'C',
                    sRBTax,
                    payment.CampusId,
                    payment.PaymentDate
                        );

                    await _unitOfWork.Ledger.Add(addSRBInRecordLedger);
                }

                if (payment.SalesTax > 0)
                {
                    var getTaxAccount = _unitOfWork.Taxes.Find(new TaxesSpecs(TaxType.SalesTaxLiability)).Select(i => i.AccountId).FirstOrDefault();
                    if (getTaxAccount == null)
                        return new Response<bool>("Sales Tax Account not found");

                    var addSalesTaxInRecordLedger = new RecordLedger(

                    transaction.Id,
                    (Guid)getTaxAccount,
                    payment.BusinessPartnerId,
                    null,
                    payment.Description,
                    'C',
                    salesTax,
                    payment.CampusId,
                    payment.PaymentDate
                        );
                    await _unitOfWork.Ledger.Add(addSalesTaxInRecordLedger);
                }

                if (payment.IncomeTax > 0)
                {
                    var getTaxAccount = _unitOfWork.Taxes.Find(new TaxesSpecs(TaxType.IncomeTaxLiability)).Select(i => i.AccountId).FirstOrDefault();
                    if (getTaxAccount == null)
                        return new Response<bool>("SRB Tax Account not found");

                    var addIncomeTaxInRecordLedger = new RecordLedger(

                    transaction.Id,
                    (Guid)getTaxAccount,
                    payment.BusinessPartnerId,
                    null,
                    payment.Description,
                    'C',
                    incomeTax,
                    payment.CampusId,
                    payment.PaymentDate);
                    await _unitOfWork.Ledger.Add(addIncomeTaxInRecordLedger);
                }

                if (payment.Deduction > 0)
                {
                    if (payment.DeductionAccountId == null)
                        return new Response<bool>("Deduction Account not found");

                    var addDeductionRecordLedger = new RecordLedger(

                    transaction.Id,
                    (Guid)payment.DeductionAccountId,
                    payment.BusinessPartnerId,
                    null,
                    payment.Description,
                    'C',
                    payment.Deduction,
                    payment.CampusId,
                    payment.PaymentDate);
                    await _unitOfWork.Ledger.Add(addDeductionRecordLedger);
                }
                var addNetPaymentInRecordLedger = new RecordLedger(
                    transaction.Id,
                    payment.PaymentRegisterId,
                    payment.BusinessPartnerId,
                    null,
                    payment.Description,
                    'C',
                    (payment.GrossPayment - salesTax - incomeTax - sRBTax - payment.Deduction),
                    payment.CampusId,
                    payment.PaymentDate);

                await _unitOfWork.Ledger.Add(addNetPaymentInRecordLedger);
            }
            await _unitOfWork.SaveAsync();

            //Getting transaction with Payment Transaction Id
            var getUnreconciledDocumentAmount = _unitOfWork.Ledger.Find(new LedgerSpecs(transaction.Id, true)).FirstOrDefault();

            if (getUnreconciledDocumentAmount != null)
                payment.setLedgerId(getUnreconciledDocumentAmount.Id);

            await _unitOfWork.SaveAsync();

            return new Response<bool>(true, "Payment Registered Successfully");
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

            // Creating object of getUSer class
            var getUser = new GetUser(this._httpContextAccessor);

            var userId = getUser.GetCurrentUserId();

            var currentUserRoles = new GetUser(this._httpContextAccessor).GetCurrentUserRoles();
            _unitOfWork.CreateTransaction();
          
                foreach (var role in currentUserRoles)
                {
                    if (transition.AllowedRole.Name == role)
                    {
                        getPayment.setStatus(transition.NextStatusId);
                        if (!String.IsNullOrEmpty(data.Remarks))
                        {
                            var addRemarks = new Remark()
                            {
                                DocId = getPayment.Id,
                                DocType = getPayment.PaymentFormType,
                                Remarks = data.Remarks,
                                UserId = userId
                            };
                            await _unitOfWork.Remarks.Add(addRemarks);
                        }
                        if (transition.NextStatus.State == DocumentStatus.Unpaid)

                        {
                            var totalAddedTax = (getPayment.IncomeTax + getPayment.SalesTax + getPayment.SRBTax);
                            getPayment.setReconStatus(DocumentStatus.Unreconciled);
                            var res = await AddToLedger(getPayment);
                            if (!res.IsSuccess)
                            {
                                _unitOfWork.Rollback();
                                return new Response<bool>(res.Message);
                            }
                            if (getPayment.DocumentLedgerId != 0 && getPayment.DocumentLedgerId != null)
                            {
                                TransactionReconcileService trecon = new TransactionReconcileService(_unitOfWork);
                                //Getting transaction with Payment Transaction Id
                                var getUnreconciledDocumentAmount = _unitOfWork.Ledger.Find(new LedgerSpecs(true, (int)getPayment.TransactionId)).FirstOrDefault();
                                if (getUnreconciledDocumentAmount == null)
                                {
                                    _unitOfWork.Rollback();
                                    return new Response<bool>("Ledger not found");
                                }

                                var reconModel = new CreateTransactionReconcileDto()
                                {
                                    PaymentLedgerId = getUnreconciledDocumentAmount.Id,
                                    DocumentLedgerId = (int)getPayment.DocumentLedgerId,
                                    Amount = getPayment.GrossPayment
                                };

                                //Checking Reconciliation Validation
                                var checkValidation = trecon.CheckReconValidation(reconModel);
                                if (!checkValidation.IsSuccess)
                                {
                                    _unitOfWork.Rollback();
                                    return new Response<bool>(checkValidation.Message);
                                }

                                var reconcile = await trecon.ReconciliationProcess(reconModel);
                                if (!reconcile.IsSuccess)
                                {
                                    _unitOfWork.Rollback();
                                    return new Response<bool>(reconcile.Message);
                                }
                            }
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
                var netPayment = e.GrossPayment - ((e.GrossPayment * e.IncomeTax)/100) - ((e.GrossPayment * e.SalesTax) / 100) - ((e.GrossPayment * e.SRBTax) / 100) - e.Deduction;
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

        public async Task<Response<List<PayrollTransactionDto>>> CreatePayrollPaymentProcess(CreatePayrollPaymentDto data)
        {
            var payrollPayment = new List<PayrollTransactionDto>();

            //checking whether any payment is created
            foreach (var line in data.CreatePayrollTransLines)
            {
                var bp = await _unitOfWork.BusinessPartner.GetById((int)line.BusinessPartnerId);
                if (bp != null)
                {
                    if (bp.BusinessPartnerType != BusinessPartnerType.Employee)
                        return new Response<List<PayrollTransactionDto>>("Business Partner is not employee");
                }

                var checkPayrollPayment = _unitOfWork.Payment.Find(new PaymentSpecs(line.LedgerId, true)).FirstOrDefault();

                if (checkPayrollPayment == null)
                {

                    var payment = new CreatePaymentDto()
                    {
                        PaymentType = PaymentType.Outflow,
                        PaymentFormType = DocType.PayrollPayment,
                        BusinessPartnerId = line.BusinessPartnerId,
                        AccountId = line.AccountPayableId,
                        CampusId = line.CampusId,
                        GrossPayment = line.NetSalary,
                        PaymentDate = data.PaymentDate,
                        PaymentRegisterType = data.PaymentRegisterType,
                        PaymentRegisterId = data.PaymentRegisterId,
                        Description = data.Description,
                        SalesTax = 0,
                        IncomeTax = 0,
                        SRBTax = 0,
                        Deduction = 0,
                        DocumentLedgerId = line.LedgerId,
                        isSubmit = false
                    };

                    if ((bool)payment.isSubmit)
                    {
                        var result = await this.SubmitPay(payment);
                        if (!result.IsSuccess)
                        {
                            return new Response<List<PayrollTransactionDto>>("");
                        }
                    }
                    else
                    {
                        var result = await this.SavePay(payment, 1);
                        if (!result.IsSuccess)
                        {
                            return new Response<List<PayrollTransactionDto>>("");
                        }
                    }
                }
            }

            if (payrollPayment.Count() > 0)
            {
                return new Response<List<PayrollTransactionDto>>("Payroll Payments already exists");
            }

            return new Response<List<PayrollTransactionDto>>(null, "Payroll Payment created successfully");
        }

        public Response<List<PayrollTransactionDto>> GetPayrollTransactionByDept(DeptFilter data)
        {
            //Fetching approved payrolltransactions
            var payrollTransactions = _unitOfWork.PayrollTransaction.Find(new PayrollTransactionSpecs((int)data.Month, (int)data.Year, data.DepartmentId, (int)data.CampusId, "")).ToList();


            //Filtering payrolltransaction which doesn't have payrollpayments.
            var filteredPayrollTransactions = new List<PayrollTransactionMaster>();

            foreach (var i in payrollTransactions)
            {
                //fetching payrollpayments which have current payrolltransaction ledgerId
                var getPayrollPayments = _unitOfWork.Payment.Find(new PaymentSpecs(DocType.PayrollPayment, (int)i.LedgerId)).FirstOrDefault();

                if (getPayrollPayments == null)
                {
                    filteredPayrollTransactions.Add(i);
                }
            }

            if (filteredPayrollTransactions.Count == 0)
                return new Response<List<PayrollTransactionDto>>(null, "list is empty");

            var response = new List<PayrollTransactionDto>();

            foreach (var i in filteredPayrollTransactions)
            {
                response.Add(new PayrollTransactionService(_unitOfWork, _mapper, _employeeService, _httpContextAccessor).MapToValue(i));
            }
            var result = response.OrderBy(i => i.Employee).ToList();

            return new Response<List<PayrollTransactionDto>>(result, "Returning Payroll Transactions");
        }

        public Response<List<PaymentDto>> GetPaymentByDept(DeptFilter data)
         {
            var payrollTransactions = _unitOfWork.PayrollTransaction.Find(new PayrollTransactionSpecs((int)data.Month, (int)data.Year, data.DepartmentId, (int)data.CampusId, "")).ToList();

            if (payrollTransactions.Count == 0)
                return new Response<List<PaymentDto>>(null, "list is empty");
            var getPayments = _unitOfWork.Payment.Find(new PaymentSpecs(DocType.PayrollPayment, false)).ToList();

            var response = new List<PaymentDto>();

            foreach (var t in payrollTransactions)
            {
                var payrollLedgerId = _unitOfWork.Ledger.Find(new LedgerSpecs(true, (int)t.TransactionId)).Select(i => i.Id).FirstOrDefault();
                var payment = getPayments.FirstOrDefault(x => x.DocumentLedgerId == payrollLedgerId);
                if (payment != null)
                {
                    response.Add(_mapper.Map<PaymentDto>(payment));
                }
            }
            var result = response.OrderBy(x => x.BusinessPartnerId).ToList();

            return new Response<List<PaymentDto>>(result, "Returning payroll payments");

        }

        public async Task<Response<bool>> ProcessForEditPayrollPayment(int[] id)
        {
            _unitOfWork.CreateTransaction();
            
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

        public Response<List<PaymentDto>> GetPaymentForApproval(DeptFilter data)
        {
            var payrollTransactions = _unitOfWork.PayrollTransaction.Find(new PayrollTransactionSpecs((int)data.Month, (int)data.Year, data.DepartmentId, (int)data.CampusId, "")).ToList();

            if (payrollTransactions.Count == 0)
                return new Response<List<PaymentDto>>(null, "list is empty");

            var getPayments = _unitOfWork.Payment.Find(new PaymentSpecs(DocType.PayrollPayment, true)).ToList();

            var response = new List<PaymentDto>();

            foreach (var t in payrollTransactions)
            {
                var payrollLedgerId = _unitOfWork.Ledger.Find(new LedgerSpecs(true, (int)t.TransactionId)).Select(i => i.Id).FirstOrDefault();
                var payment = getPayments.FirstOrDefault(x => x.DocumentLedgerId == payrollLedgerId);
                if (payment != null)
                {
                    response.Add(_mapper.Map<PaymentDto>(payment));
                }
            }

            var result = response.OrderBy(x => x.BusinessPartnerId).ToList();

            return new Response<List<PaymentDto>>(result, "Returning payroll payments");
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

        private PaymentDto MapToValue(PaymentDto data)
        {
            //Getting transaction with Payment Transaction Id
            var getUnreconciledDocumentAmount = _unitOfWork.Ledger.Find(new LedgerSpecs((int)data.TransactionId, true)).FirstOrDefault();

            // Checking if given amount is greater than unreconciled document amount
            var transactionReconciles = _unitOfWork.TransactionReconcile.Find(new TransactionReconSpecs(getUnreconciledDocumentAmount.Id, true)).ToList();

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
                        Id = tranRecon.DocumentLedger.Transactions.DocId,
                        DocNo = tranRecon.DocumentLedger.Transactions.DocNo,
                        DocType = tranRecon.DocumentLedger.Transactions.DocType,
                        Amount = tranRecon.Amount
                    });
                }
            }

            //Getting Pending Invoice Amount
            var unReconciledAmount = data.NetPayment - transactionReconciles.Sum(e => e.Amount);

            data.ReconciledAmount = transactionReconciles.Sum(e => e.Amount);
            data.PaidAmountList = paidDocList;
            data.UnreconciledAmount = unReconciledAmount;

            // Returning BillDto with all values assigned
            return data;
        }

        private List<RemarksDto> ReturningRemarks(PaymentDto data, DocType docType)
        {
            var remarks = _unitOfWork.Remarks.Find(new RemarksSpecs(data.Id, docType))
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

        private List<FileUploadDto> ReturningFiles(PaymentDto data, DocType docType)
        {

            var files = _unitOfWork.Fileupload.Find(new FileUploadSpecs(data.Id, docType))
                    .Select(e => new FileUploadDto()
                    {
                        Id = e.Id,
                        Name = e.Name,
                        DocType = docType,
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
    }
}
