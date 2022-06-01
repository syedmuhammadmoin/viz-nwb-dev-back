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
        private readonly IMapper _mapper;

        public TransactionReconcileService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        //public async Task<Response<bool>> Reconcile(CreateTransactionReconcileDto entity)
        //{

        //    //Declaring Variables
        //    dynamic paymentTotalAmount;
        //    dynamic invoiceTotalAmount;
        //    dynamic creditNoteTotalAmount = 0;
        //    dynamic debitNoteTotalAmount = 0;
        //    dynamic billTotalAmount = 0;


        //    // FOR PAYMENT
        //    var paymentAmounts = GetPaymentReconAmounts(entity.PaymentTransactionId);
        //    paymentTotalAmount = paymentAmounts.TotalAmount;

        //    if (entity.Amount > paymentAmounts.UnreconciledAmount)
        //    {
        //        //FOR CREDIT NOTE
        //        var creditNoteAmounts = GetCreditNoteReconAmounts(entity.PaymentTransactionId);
        //        creditNoteTotalAmount = creditNoteAmounts.TotalAmount;

        //        if (entity.Amount > creditNoteAmounts.UnreconciledAmount)
        //        {
        //            //FOR DEBIT NOTE
        //            var debitNoteAmounts = GetDebitNoteReconAmounts(entity.PaymentTransactionId);
        //            debitNoteTotalAmount = debitNoteAmounts.TotalAmount;

        //            if (entity.Amount > debitNoteAmounts.UnreconciledAmount)
        //            {
        //                return new Response<bool>("Enter amount is greater than pending payment amount");
        //            }
        //        }
        //    }

        //    //FOR INVOICE
        //    var invoiceAmounts = GetInvoiceReconAmounts(entity.DocumentTransactionId);
        //    invoiceTotalAmount = invoiceAmounts.TotalAmount;

        //    if (entity.Amount > invoiceAmounts.UnreconciledAmount)
        //    {
        //        //FOR BILL
        //        var billAmounts = GetBillReconAmounts(entity.DocumentTransactionId);
        //        billTotalAmount = billAmounts.TotalAmount;

        //        if (entity.Amount > billAmounts.UnreconciledAmount)
        //        {
        //            return new Response<bool>("Enter amount is greater than pending document amount");
        //        }
        //    }

        //    //Begin Transaction
        //    _unitOfWork.CreateTransaction();
        //    try
        //    {
        //        //Adding in Reconcilation table
        //        var recons = _mapper.Map<TransactionReconcile>(entity);

        //        await _unitOfWork.TransactionReconcile.Reconcile(recons);
        //        await _unitOfWork.SaveAsync();


        //        //Get Paymet Total Reconciled Amount
        //        var reconciledTotalPayment = _unitOfWork.TransactionReconcile
        //            .Find(new TransactionReconSpecs(entity.PaymentTransactionId, true))
        //            .Sum(i => i.Amount);

        //        //FOR PAYMENT STATUS
        //        var payment = _unitOfWork.Payment.Find(new PaymentSpecs(entity.PaymentTransactionId)).FirstOrDefault();

        //        if (payment != null)
        //        {
        //            if ((decimal)paymentTotalAmount == reconciledTotalPayment)
        //            {
        //                payment.setStatus(5); // Paid
        //            }
        //            else
        //            {
        //                payment.setStatus(4); // Partial
        //            }
        //        }

        //        //FOR CREDITNOTE STATUS
        //        var creditNote = _unitOfWork.CreditNote.Find(new CreditNoteSpecs(entity.PaymentTransactionId)).FirstOrDefault();

        //        if (creditNote != null)
        //        {
        //            if (creditNoteTotalAmount == reconciledTotalPayment)
        //            {
        //                creditNote.setStatus(5); // Paid
        //            }
        //            else
        //            {
        //                creditNote.setStatus(4); // Partial
        //            }
        //        }

        //        //FOR DEBITNOTE STATUS

        //        var debitNote = _unitOfWork.DebitNote.Find(new DebitNoteSpecs(entity.PaymentTransactionId)).FirstOrDefault();

        //        if (debitNote != null)
        //        {
        //            if (debitNoteTotalAmount == reconciledTotalPayment)
        //            {
        //                debitNote.StatusId = 5; // Paid
        //            }
        //            else
        //            {
        //                debitNote.StatusId = 4; // Partial
        //            }
        //        }

        //        //Get Document Total Reconciled Amount
        //        var reconciledTotalDocAmount = _unitOfWork.TransactionReconcile
        //            .Find(new TransactionReconSpecs(entity.DocumentTransactionId, false))
        //            .Sum(i => i.Amount);


        //        //FOR INVOICE STATUS
        //        var invoice = _unitOfWork.Invoice.Find(new InvoiceSpecs(entity.DocumentTransactionId)).FirstOrDefault();

        //        if (invoice != null)
        //        {
        //            if (invoiceTotalAmount == reconciledTotalDocAmount)
        //            {
        //                invoice.setStatus(5); // Paid
        //            }
        //            else
        //            {
        //                invoice.setStatus(4); // Partial
        //            }
        //        }

        //        //FOR BILL STATUS
        //        var bill = _unitOfWork.Bill.Find(new BillSpecs(entity.DocumentTransactionId)).FirstOrDefault();

        //        if (bill != null)
        //        {
        //            if (billTotalAmount == reconciledTotalDocAmount)
        //            {
        //                bill.setStatus(5); // Paid;
        //            }
        //            else
        //            {
        //                bill.setStatus(4); // Partial
        //            }
        //        }

        //        await _unitOfWork.SaveAsync();
        //        _unitOfWork.Commit();

        //        return new Response<bool>(true, "Reconciled Successfully");
        //    }
        //    catch (Exception ex)
        //    {
        //        _unitOfWork.Rollback();
        //        return new Response<bool>(ex.Message);
        //    }
        //}

        public async Task<Response<bool>> Reconcile(CreateTransactionReconcileDto entity)
        {
            //Checking if both id are same
            if (entity.DocumentTransactionId == entity.PaymentTransactionId)
                return new Response<bool>("Both transaction id cannot be same");

            //Getting transaction with Payment Transaction Id
            var getUnreconciledDocumentAmount = _unitOfWork.Ledger.Find(new LedgerSpecs(entity.DocumentTransactionId)).FirstOrDefault();
            if (getUnreconciledDocumentAmount == null)
                return new Response<bool>("No Transaction found for the given document transaction id");

            //Getting transaction with Document Transaction Id
            var getUnreconciledPaymentAmount = _unitOfWork.Ledger.Find(new LedgerSpecs(entity.PaymentTransactionId,
                getUnreconciledDocumentAmount.Level4_id, getUnreconciledDocumentAmount.BusinessPartnerId, getUnreconciledDocumentAmount.Sign)).FirstOrDefault();
            if (getUnreconciledPaymentAmount == null)
                return new Response<bool>("No Transaction found for the given payment transaction id");

            // Checking if given amount is greater than unreconciled document amount
            var reconciledDocumentAmount = _unitOfWork.TransactionReconcile.Find(new TransactionReconSpecs(entity.DocumentTransactionId, true)).Sum(p => p.Amount);
            var unreconciledDocumentAmount = getUnreconciledDocumentAmount.Amount - reconciledDocumentAmount;
            if (entity.Amount > unreconciledDocumentAmount)
                return new Response<bool>("Enter amount is greater than pending document amount");

            // Checking if given amount is greater than unreconciled payment amount
            var reconciledPaymentAmount = _unitOfWork.TransactionReconcile.Find(new TransactionReconSpecs(entity.PaymentTransactionId, true)).Sum(p => p.Amount);
            var unreconciledPaymentAmount = getUnreconciledPaymentAmount.Amount - reconciledPaymentAmount;
            if (entity.Amount > unreconciledPaymentAmount)
                return new Response<bool>("Enter amount is greater than pending payment amount");


            //Begin Transaction
            _unitOfWork.CreateTransaction();
            try
            {
                //Adding in Reconcilation table
                var recons = new TransactionReconcile(getUnreconciledPaymentAmount.Id, getUnreconciledDocumentAmount.Id, entity.Amount);
                await _unitOfWork.TransactionReconcile.Reconcile(recons);
                await _unitOfWork.SaveAsync();


                //Get Paymet Total Reconciled Amount
                var reconciledTotalPayment = _unitOfWork.TransactionReconcile
                    .Find(new TransactionReconSpecs(getUnreconciledPaymentAmount.Id, true))
                    .Sum(i => i.Amount);

                //FOR UPDATE STATUS
                if (getUnreconciledPaymentAmount.Amount == reconciledTotalPayment)
                {
                    getUnreconciledPaymentAmount.setStatus(DocumentStatus.Reconciled);
                    switch (getUnreconciledPaymentAmount.Transactions.DocType)
                    {
                        case Domain.Constants.DocType.Payment:
                            var payment = _unitOfWork.Payment.Find(new PaymentSpecs(entity.PaymentTransactionId)).FirstOrDefault();
                            payment.setStatus(5); // Paid
                            break;
                        case Domain.Constants.DocType.CreditNote:
                            var creaditNote = _unitOfWork.CreditNote.Find(new CreditNoteSpecs(entity.PaymentTransactionId)).FirstOrDefault();
                            creaditNote.setStatus(5); // Paid
                            break;
                        case Domain.Constants.DocType.DebitNote:
                            var debitNote = _unitOfWork.DebitNote.Find(new DebitNoteSpecs(entity.PaymentTransactionId)).FirstOrDefault();
                            debitNote.setStatus(5); // Paid
                            break;
                    }
                }
                else
                {
                    getUnreconciledPaymentAmount.setStatus(DocumentStatus.Partial);
                    switch (getUnreconciledPaymentAmount.Transactions.DocType)
                    {
                        case Domain.Constants.DocType.Payment:
                            var payment = _unitOfWork.Payment.Find(new PaymentSpecs(entity.PaymentTransactionId)).FirstOrDefault();
                            payment.setStatus(4); // Partial
                            break;
                        case Domain.Constants.DocType.CreditNote:
                            var creaditNote = _unitOfWork.CreditNote.Find(new CreditNoteSpecs(entity.PaymentTransactionId)).FirstOrDefault();
                            creaditNote.setStatus(4); // Partial
                            break;
                        case Domain.Constants.DocType.DebitNote:
                            var debitNote = _unitOfWork.DebitNote.Find(new DebitNoteSpecs(entity.PaymentTransactionId)).FirstOrDefault();
                            debitNote.setStatus(4); // Partial
                            break;
                    }
                }

                //Get Document Total Reconciled Amount
                var reconciledTotalDocAmount = _unitOfWork.TransactionReconcile
                    .Find(new TransactionReconSpecs(getUnreconciledDocumentAmount.Id, false))
                    .Sum(i => i.Amount);

                //FOR UPDATE STATUS
                if (getUnreconciledDocumentAmount.Amount == reconciledTotalPayment)
                {
                    getUnreconciledDocumentAmount.setStatus(DocumentStatus.Reconciled);
                    switch (getUnreconciledDocumentAmount.Transactions.DocType)
                    {
                        case Domain.Constants.DocType.Invoice:
                            var invoice = _unitOfWork.Invoice.Find(new InvoiceSpecs(entity.DocumentTransactionId)).FirstOrDefault();
                            invoice.setStatus(5); // Paid
                            break;
                        case Domain.Constants.DocType.Bill:
                            var bill = _unitOfWork.Bill.Find(new BillSpecs(entity.DocumentTransactionId)).FirstOrDefault();
                            bill.setStatus(5); // Paid
                            break;
                    }
                }
                else
                {
                    getUnreconciledDocumentAmount.setStatus(DocumentStatus.Partial);
                    switch (getUnreconciledDocumentAmount.Transactions.DocType)
                    {
                        case Domain.Constants.DocType.Invoice:
                            var invoice = _unitOfWork.Invoice.Find(new InvoiceSpecs(entity.DocumentTransactionId)).FirstOrDefault();
                            invoice.setStatus(4); // Paid
                            break;
                        case Domain.Constants.DocType.Bill:
                            var bill = _unitOfWork.Bill.Find(new BillSpecs(entity.DocumentTransactionId)).FirstOrDefault();
                            bill.setStatus(4); // Paid
                            break;
                    }
                }
                await _unitOfWork.SaveAsync();
                _unitOfWork.Commit();

                return new Response<bool>(true, "Reconciled Successfully");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new Response<bool>(ex.Message);
            }
        }

        public AmountsForReconciliationDto GetPaymentReconAmounts(int transactionId)
        {
            // FOR PAYMENT
            var paymentTotalAmount = _unitOfWork.Payment.Find(new PaymentSpecs(transactionId)).Select(e => e.GrossPayment).FirstOrDefault();
            
            var reconciledAmount = _unitOfWork.TransactionReconcile.Find(new TransactionReconSpecs(transactionId, true)).Sum(p => p.Amount);

            var unreconciledAmount = paymentTotalAmount - reconciledAmount;
            return new AmountsForReconciliationDto
            {
                TotalAmount = paymentTotalAmount,
                ReconciledAmount = reconciledAmount,
                UnreconciledAmount = unreconciledAmount
            };
        }

        public AmountsForReconciliationDto GetCreditNoteReconAmounts(int transactionId)
        {
            //FOR CREDIT NOTE
            var creditNoteTotalAmount = _unitOfWork.CreditNote.Find(new CreditNoteSpecs(transactionId)).Select(e => e.TotalAmount).FirstOrDefault();
            
            var reconciledCreditNoteAmount = _unitOfWork.TransactionReconcile.Find(new TransactionReconSpecs(transactionId, true)).Sum(p => p.Amount);

            var unreconciledCreditNoteAmount = creditNoteTotalAmount - reconciledCreditNoteAmount;
            return new AmountsForReconciliationDto
            {
                TotalAmount = creditNoteTotalAmount,
                ReconciledAmount = reconciledCreditNoteAmount,
                UnreconciledAmount = unreconciledCreditNoteAmount
            };
        }
        
        public AmountsForReconciliationDto GetDebitNoteReconAmounts(int transactionId)
        {
            //FOR DEBIT NOTE
            var debitNoteTotalAmount = _unitOfWork.DebitNote.Find(new DebitNoteSpecs(transactionId)).Select(e => e.TotalAmount).FirstOrDefault();

            var reconciledDebitNoteAmount = _unitOfWork.TransactionReconcile.Find(new TransactionReconSpecs(transactionId, true)).Sum(p => p.Amount);

            var unreconciledDebitNoteAmount = debitNoteTotalAmount - reconciledDebitNoteAmount;
            return new AmountsForReconciliationDto
            {
                TotalAmount = debitNoteTotalAmount,
                ReconciledAmount = reconciledDebitNoteAmount,
                UnreconciledAmount = unreconciledDebitNoteAmount
            };
        }


        public AmountsForReconciliationDto GetInvoiceReconAmounts(int transactionId)
        {
            //FOR INVOICE
            var invoiceTotalAmount = _unitOfWork.Invoice.Find(new InvoiceSpecs(transactionId)).Select(e => e.TotalAmount).FirstOrDefault();
            
            var reconciledInvoiceAmount = _unitOfWork.TransactionReconcile.Find(new TransactionReconSpecs(transactionId, false)).Sum(p => p.Amount);

            var unreconciledInvoiceAmount = invoiceTotalAmount - reconciledInvoiceAmount;
            return new AmountsForReconciliationDto
            {
                TotalAmount = invoiceTotalAmount,
                ReconciledAmount = reconciledInvoiceAmount,
                UnreconciledAmount = unreconciledInvoiceAmount
            };
        }

        public AmountsForReconciliationDto GetBillReconAmounts(int transactionId)
        {
            //FOR BILL
            var billTotalAmount = _unitOfWork.Bill.Find(new BillSpecs(transactionId)).Select(e => e.TotalAmount).FirstOrDefault();
            
            var reconciledBillAmount = _unitOfWork.TransactionReconcile.Find(new TransactionReconSpecs(transactionId, false)).Sum(p => p.Amount);

            var unreconciledBillAmount = billTotalAmount - reconciledBillAmount;
            return new AmountsForReconciliationDto
            {
                TotalAmount = billTotalAmount,
                ReconciledAmount = reconciledBillAmount,
                UnreconciledAmount = unreconciledBillAmount
            };
        }

    }
}
