using Application.Contracts.DTOs;
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
    public class BankReconService : IBankReconService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BankReconService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<int>> CreateAsync(CreateBankReconDto[] entity)
        {
            foreach (var r in entity)
            {
                var result = await BankReconciliationProcess(r);
                if (!result.IsSuccess)
                    return new Response<int>(result.Message);
            }
            return new Response<int>(1, "Reconciled successfully");
        }

        private async Task<Response<int>> BankReconciliationProcess(CreateBankReconDto entity)
        {
            if (entity.Amount < 0)
                return new Response<int>("Amount is negative");


            // FOR BANK STATEMENT
            var bankStmtLine = await _unitOfWork.BankStmtLines.GetById(entity.BankStmtId, new BankStmtLinesSpecs());
            decimal stmtAmount = bankStmtLine != null ? (bankStmtLine.Credit - bankStmtLine.Debit) < 0 ? (bankStmtLine.Credit - bankStmtLine.Debit) * -1
                : bankStmtLine.Credit - bankStmtLine.Debit : 0;

            var reconciledStmtAmount = _unitOfWork.BankReconciliation.Find(new BankReconSpecs(entity.BankStmtId, false)).Sum(i => i.Amount);
            var unreconciledStmtAmount = stmtAmount - reconciledStmtAmount;

            if (entity.Amount > unreconciledStmtAmount)
                return new Response<int>("Amount is greater than Unreconciled Statment Amount");

            // FOR PAYMENT
            var payment = await _unitOfWork.Payment.GetById(entity.PaymentId, new PaymentSpecs(true));
            decimal paymentTotalAmount = payment.GrossPayment - payment.Discount - payment.IncomeTax - payment.SalesTax;
            var reconciledPaymentAmount = _unitOfWork.BankReconciliation.Find(new BankReconSpecs(entity.PaymentId, true)).Sum(i => i.Amount);
            decimal unreconciledPaymentAmount = paymentTotalAmount - reconciledPaymentAmount;

            if (entity.Amount > unreconciledPaymentAmount)
                return new Response<int>("Amount is greater than Unreconciled Payment Amount");

            _unitOfWork.CreateTransaction();
            try
            {
                var bankRecon = _mapper.Map<BankReconciliation>(entity);
                await _unitOfWork.BankReconciliation.Add(bankRecon);
                await _unitOfWork.SaveAsync();

                //FOR BANK STATEMENT STATUS
                decimal reconciledTotalStmtAmount = _unitOfWork.BankReconciliation.Find(new BankReconSpecs(entity.BankStmtId, false)).Sum(i => i.Amount);
                var bankStmtLineforUpdate = await _unitOfWork.BankStmtLines.GetById(entity.BankStmtId);

                if (bankStmtLineforUpdate != null)
                {
                    if (stmtAmount == reconciledTotalStmtAmount)
                    {
                        bankStmtLine.updateStatus(DocumentStatus.Reconciled);
                    }
                    else
                    {
                        bankStmtLine.updateStatus(DocumentStatus.Partial);
                    }
                }

                //FOR PAYMENT STATUS
                decimal reconciledTotalPayment = _unitOfWork.BankReconciliation.Find(new BankReconSpecs(entity.PaymentId, true)).Sum(i => i.Amount);

                if (paymentTotalAmount == reconciledTotalPayment)
                {
                    payment.setReconStatus(DocumentStatus.Reconciled);

                    var bankAccount = _unitOfWork.BankAccount.Find(new BankAccountSpecs(payment.PaymentRegisterId)).FirstOrDefault();

                    if (payment.PaymentType == PaymentType.Inflow)
                    {
                        //Add total payment in originalBank Account Ledger
                        var addNetPaymentInRecordLedgerChAccount = new RecordLedger(
                            (int)payment.TransactionId,
                            bankAccount.ChAccountId,
                            payment.BusinessPartnerId,
                            null,
                            payment.Description,
                            'D',
                            paymentTotalAmount,
                            payment.CampusId,
                            payment.PaymentDate
                            );

                        await _unitOfWork.Ledger.Add(addNetPaymentInRecordLedgerChAccount);

                        //Add total payment in clearing Account Ledger
                        var addNetPaymentInRecordLedgerClrAccount = new RecordLedger(
                           (int)payment.TransactionId,
                           bankAccount.ClearingAccountId,
                           payment.BusinessPartnerId,
                           null,
                           payment.Description,
                           'C',
                           paymentTotalAmount,
                            payment.CampusId,
                            payment.PaymentDate
                           );

                        await _unitOfWork.Ledger.Add(addNetPaymentInRecordLedgerClrAccount);
                    }
                    if (payment.PaymentType == PaymentType.Outflow)
                    {
                        //Add total payment in originalBank Account Ledger
                        var addNetPaymentInRecordLedgerChAccount = new RecordLedger(
                            (int)payment.TransactionId,
                            bankAccount.ChAccountId,
                            payment.BusinessPartnerId,
                            null,
                            payment.Description,
                            'C',
                            paymentTotalAmount,
                           payment.CampusId,
                           payment.PaymentDate
                            );

                        await _unitOfWork.Ledger.Add(addNetPaymentInRecordLedgerChAccount);

                        //Add total payment in clearing Account Ledger
                        var addNetPaymentInRecordLedgerClrAccount = new RecordLedger(
                          (int)payment.TransactionId,
                          bankAccount.ClearingAccountId,
                          payment.BusinessPartnerId,
                          null,
                          payment.Description,
                          'D',
                          paymentTotalAmount,
                            payment.CampusId,
                            payment.PaymentDate
                          );

                        await _unitOfWork.Ledger.Add(addNetPaymentInRecordLedgerClrAccount);
                    }
                }
                else
                {
                    payment.setReconStatus(DocumentStatus.Partial);
                }
                await _unitOfWork.SaveAsync();
                _unitOfWork.Commit();
                return new Response<int>(1, "Reconciled successfully");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new Response<int>(ex.Message);
            }
        }
    }
}
