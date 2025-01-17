﻿using Application.Contracts.DTOs;
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
    public class TransactionReconcileService : ITransactionReconcileService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TransactionReconcileService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<bool>> Reconcile(CreateTransactionReconcileDto entity)
        {
            //Checking Reconciliation Validation
            var checkValidation = CheckReconValidation(entity);
            if (!checkValidation.IsSuccess)
                return new Response<bool>(checkValidation.Message);


            //Begin Transaction
            _unitOfWork.CreateTransaction();
          
                var reconcile = await ReconciliationProcess(entity);
                if (!reconcile.IsSuccess)
                {
                    _unitOfWork.Rollback();
                    return new Response<bool>(reconcile.Message);
                }
                _unitOfWork.Commit();

                return new Response<bool>(true, "Reconciled Successfully");
           
        }


        public Response<bool> CheckReconValidation(CreateTransactionReconcileDto entity)
        {
            //Checking if both id are same
            if (entity.DocumentLedgerId == entity.PaymentLedgerId)
                return new Response<bool>("Both Ledger id cannot be same");

            //Getting transaction with Payment Transaction Id
            var getUnreconciledDocumentAmount = _unitOfWork.Ledger.Find(new LedgerSpecs((int)entity.DocumentLedgerId)).FirstOrDefault();
            if (getUnreconciledDocumentAmount == null)
                return new Response<bool>("No Transaction found for the given document transaction id");

            // Checking if given amount is greater than unreconciled document amount
            var reconciledDocumentAmount = _unitOfWork.TransactionReconcile.Find(new TransactionReconSpecs((int)entity.DocumentLedgerId, false)).Sum(p => p.Amount);
            var unreconciledDocumentAmount = getUnreconciledDocumentAmount.Amount - reconciledDocumentAmount;
            if (entity.Amount > unreconciledDocumentAmount)
                return new Response<bool>("Enter amount is greater than pending document amount");

            //Getting transaction with Document Transaction Id
            var getUnreconciledPaymentAmount = _unitOfWork.Ledger.Find(new LedgerSpecs((int)entity.PaymentLedgerId,
                getUnreconciledDocumentAmount.Level4_id, getUnreconciledDocumentAmount.BusinessPartnerId, getUnreconciledDocumentAmount.Sign)).FirstOrDefault();
            if (getUnreconciledDocumentAmount == null)
                return new Response<bool>("No Transaction found for the given payment transaction id");

            // Checking if given amount is greater than unreconciled payment amount
            var reconciledPaymentAmount = _unitOfWork.TransactionReconcile.Find(new TransactionReconSpecs((int)entity.PaymentLedgerId, true)).Sum(p => p.Amount);
            var unreconciledPaymentAmount = getUnreconciledPaymentAmount.Amount - reconciledPaymentAmount;
            if (entity.Amount > unreconciledPaymentAmount)
                return new Response<bool>("Enter amount is greater than pending payment amount");


            return new Response<bool>(true, "No validation error found");
        }

        public async Task<Response<bool>> ReconciliationProcess(CreateTransactionReconcileDto entity)
        {
            //Adding in Reconcilation table
            var recons = new TransactionReconcile((int)entity.PaymentLedgerId, (int)entity.DocumentLedgerId, (decimal)entity.Amount);
            await _unitOfWork.TransactionReconcile.Reconcile(recons);
            await _unitOfWork.SaveAsync();

            //Get Paymet Total Reconciled Amount
            var reconciledTotalPayment = _unitOfWork.TransactionReconcile
                .Find(new TransactionReconSpecs((int)entity.PaymentLedgerId, true))
                .Sum(i => i.Amount);

            //FOR UPDATE STATUS
            await UpdateStatus((int)entity.PaymentLedgerId, reconciledTotalPayment);
            await _unitOfWork.SaveAsync();

            //Get Document Total Reconciled Amount
            var reconciledTotalDocAmount = _unitOfWork.TransactionReconcile
                .Find(new TransactionReconSpecs((int)entity.DocumentLedgerId, false))
                .Sum(i => i.Amount);

            //FOR UPDATE STATUS
            await UpdateStatus((int)entity.DocumentLedgerId, reconciledTotalDocAmount);

            await _unitOfWork.SaveAsync();

            return new Response<bool>(true, "Reconciled Successfully");
        }

        private async Task UpdateStatus(int ledgerId, decimal reconciledTotalDocument)
        {
            var updateLedger = await _unitOfWork.Ledger.GetById(ledgerId, new GetLedgerByIdSpecs());
          
            //FOR UPDATE STATUS
            if (updateLedger.Amount == reconciledTotalDocument)
            {
                updateLedger.SetStatus(DocumentStatus.Reconciled);
                switch (updateLedger.Transactions.DocType)
                {
                    case DocType.Payment:
                        var payment = _unitOfWork.Payment.Find(new PaymentSpecs(updateLedger.TransactionId)).FirstOrDefault();
                        payment.SetStatus(5); // Paid
                        break;
                    case DocType.Receipt:
                        var receipt = _unitOfWork.Payment.Find(new PaymentSpecs(updateLedger.TransactionId)).FirstOrDefault();
                        receipt.SetStatus(5); // Paid
                        break;
                    case DocType.PayrollPayment:
                        var payrollPayment = _unitOfWork.Payment.Find(new PaymentSpecs(updateLedger.TransactionId)).FirstOrDefault();
                        payrollPayment.SetStatus(5); // Paid
                        break;
                    case DocType.CreditNote:
                        var creaditNote = _unitOfWork.CreditNote.Find(new CreditNoteSpecs(updateLedger.TransactionId)).FirstOrDefault();
                        creaditNote.SetStatus(5); // Paid
                        break;
                    case DocType.DebitNote:
                        var debitNote = _unitOfWork.DebitNote.Find(new DebitNoteSpecs(updateLedger.TransactionId)).FirstOrDefault();
                        debitNote.SetStatus(5); // Paid
                        break;
                    case DocType.Invoice:
                        var invoice = _unitOfWork.Invoice.Find(new InvoiceSpecs(updateLedger.TransactionId)).FirstOrDefault();
                        invoice.SetStatus(5); // Paid
                        break;
                    case DocType.Bill:
                        var bill = _unitOfWork.Bill.Find(new BillSpecs(updateLedger.TransactionId)).FirstOrDefault();
                        bill.SetStatus(5); // Paid
                        break;
                    case DocType.PayrollTransaction:
                        var payrollTransaction = _unitOfWork.PayrollTransaction.Find(new PayrollTransactionSpecs(updateLedger.TransactionId)).FirstOrDefault();
                        payrollTransaction.SetStatus(5); // Paid
                        break;
                    case DocType.Disposal:
                        var disposal = _unitOfWork.Disposal.Find(new DisposalSpecs(updateLedger.TransactionId)).FirstOrDefault();
                        disposal.SetStatus(5); // Paid
                        break;
                }
            }
            else
            {
                updateLedger.SetStatus(DocumentStatus.Partial);
                switch (updateLedger.Transactions.DocType)
                {
                    case DocType.Payment:
                        var payment = _unitOfWork.Payment.Find(new PaymentSpecs(updateLedger.TransactionId)).FirstOrDefault();
                        payment.SetStatus(4); // Partial
                        break;
                    case DocType.Receipt:
                        var receipt = _unitOfWork.Payment.Find(new PaymentSpecs(updateLedger.TransactionId)).FirstOrDefault();
                        receipt.SetStatus(4); // Partial
                        break;
                    case DocType.PayrollPayment:
                        var payrollPayment = _unitOfWork.Payment.Find(new PaymentSpecs(updateLedger.TransactionId)).FirstOrDefault();
                        payrollPayment.SetStatus(4); // Partial
                        break;
                    case DocType.CreditNote:
                        var creaditNote = _unitOfWork.CreditNote.Find(new CreditNoteSpecs(updateLedger.TransactionId)).FirstOrDefault();
                        creaditNote.SetStatus(4); // Partial
                        break;
                    case DocType.DebitNote:
                        var debitNote = _unitOfWork.DebitNote.Find(new DebitNoteSpecs(updateLedger.TransactionId)).FirstOrDefault();
                        debitNote.SetStatus(4); // Partial
                        break;
                    case DocType.Invoice:
                        var invoice = _unitOfWork.Invoice.Find(new InvoiceSpecs(updateLedger.TransactionId)).FirstOrDefault();
                        invoice.SetStatus(4); // Partial
                        break;
                    case DocType.Bill:
                        var bill = _unitOfWork.Bill.Find(new BillSpecs(updateLedger.TransactionId)).FirstOrDefault();
                        bill.SetStatus(4); // Partial
                        break;
                    case DocType.PayrollTransaction:
                        var payrollTransaction = _unitOfWork.PayrollTransaction.Find(new PayrollTransactionSpecs(updateLedger.TransactionId)).FirstOrDefault();
                        payrollTransaction.SetStatus(4); // Partial
                        break;
                    case DocType.Disposal:
                        var disposal = _unitOfWork.Disposal.Find(new DisposalSpecs(updateLedger.TransactionId)).FirstOrDefault();
                        disposal.SetStatus(4); // Partial
                        break;
                }
            }
        }


        public Response<List<AmountsForReconciliationDto>> GetPaymentReconAmounts(string accountId, int businessPartnerId, char sign)
        {
            var amountsForRecociliationList = new List<AmountsForReconciliationDto>();
            var getUnreconciledPaymentAmountList = _unitOfWork.Ledger.Find(new LedgerSpecs(accountId, businessPartnerId, sign)).ToList();

            if (getUnreconciledPaymentAmountList.Any())
            {
                foreach (var line in getUnreconciledPaymentAmountList)
                {
                    var reconciledPaymentAmount = _unitOfWork.TransactionReconcile.Find(new TransactionReconSpecs(line.Id, true)).Sum(p => p.Amount);
                    var unreconciledPaymentAmount = line.Amount - reconciledPaymentAmount;
                    
                    amountsForRecociliationList.Add(new AmountsForReconciliationDto
                    {
                        DocId = line.Transactions.DocId,
                        PaymentLedgerId = line.Id,
                        DocNo = line.Transactions.DocNo,
                        DocType = line.Transactions.DocType,
                        TotalAmount = line.Amount,
                        ReconciledAmount = reconciledPaymentAmount,
                        UnreconciledAmount = unreconciledPaymentAmount
                    });
                }
            }
            return new Response<List<AmountsForReconciliationDto>>(amountsForRecociliationList, "Return amounts");
        }
    }
}
