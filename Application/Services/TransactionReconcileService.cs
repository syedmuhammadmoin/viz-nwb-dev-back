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
    public class TransactionReconcileService : ITransactionReconcileService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TransactionReconcileService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<bool>> Reconcile(CreateTransactionReconcileDto entity)
        {
            //Checking if both id are same
            if (entity.DocumentTransactionId == entity.PaymentTransactionId)
                return new Response<bool>("Both transaction id cannot be same");

            //Getting transaction with Payment Transaction Id
            var getUnreconciledDocumentAmount = _unitOfWork.Ledger.Find(new LedgerSpecs(entity.DocumentTransactionId)).FirstOrDefault();
            if (getUnreconciledDocumentAmount == null)
                return new Response<bool>("No Transaction found for the given document transaction id");
            
            // Checking if given amount is greater than unreconciled document amount
            var reconciledDocumentAmount = _unitOfWork.TransactionReconcile.Find(new TransactionReconSpecs(getUnreconciledDocumentAmount.Id, false)).Sum(p => p.Amount);
            var unreconciledDocumentAmount = getUnreconciledDocumentAmount.Amount - reconciledDocumentAmount;
            if (entity.Amount > unreconciledDocumentAmount)
                return new Response<bool>("Enter amount is greater than pending document amount");


            //Getting transaction with Document Transaction Id
            var getUnreconciledPaymentAmount = _unitOfWork.Ledger.Find(new LedgerSpecs(entity.PaymentTransactionId,
                getUnreconciledDocumentAmount.Level4_id, getUnreconciledDocumentAmount.BusinessPartnerId, getUnreconciledDocumentAmount.Sign)).ToList();
            if (!getUnreconciledPaymentAmount.Any())
                return new Response<bool>("No Transaction found for the given payment transaction id");

            // declaring list for unconciledPaymentAmounts
            var unreconciledPaymentAmountList = new List<PaymentAmountListDto>();
            decimal unreconciledPaymentAmountTotal = 0;

            // looping thourgh unreconciled amount for adding them to list
            foreach (var item in getUnreconciledPaymentAmount)
            {
                var reconciledPaymentAmount = _unitOfWork.TransactionReconcile.Find(new TransactionReconSpecs(item.Id, true)).Sum(p => p.Amount);
                unreconciledPaymentAmountTotal += item.Amount - reconciledPaymentAmount;
                //if (entity.Amount > unreconciledPaymentAmount)
                //    return new Response<bool>("Enter amount is greater than pending payment amount");

                unreconciledPaymentAmountList.Add(new PaymentAmountListDto()
                {
                    LedgerId = item.Id,
                    UnreconciledAmount = item.Amount - reconciledPaymentAmount,
                });
            }

            // Checking if given amount is greater than unreconciled payment amount
            if (entity.Amount > unreconciledPaymentAmountTotal)
                return new Response<bool>("Enter amount is greater than pending payment amount");

            //Begin Transaction
            _unitOfWork.CreateTransaction();
            try
            {
                foreach (var item in unreconciledPaymentAmountList)
                {
                    //Adding in Reconcilation table
                    var recons = new TransactionReconcile(item.LedgerId, getUnreconciledDocumentAmount.Id, item.UnreconciledAmount);
                    await _unitOfWork.TransactionReconcile.Reconcile(recons);
                    await _unitOfWork.SaveAsync();

                    //Get Paymet Total Reconciled Amount
                    var reconciledTotalPayment = _unitOfWork.TransactionReconcile
                        .Find(new TransactionReconSpecs(item.LedgerId, true))
                        .Sum(i => i.Amount);

                    //FOR UPDATE STATUS
                    await UpdateStatus(getUnreconciledPaymentAmount.FirstOrDefault(i => i.Id == item.LedgerId), reconciledTotalPayment, entity.PaymentTransactionId);
                    await _unitOfWork.SaveAsync();
                }

                //Get Document Total Reconciled Amount
                var reconciledTotalDocAmount = _unitOfWork.TransactionReconcile
                    .Find(new TransactionReconSpecs(getUnreconciledDocumentAmount.Id, false))
                    .Sum(i => i.Amount);

                //FOR UPDATE STATUS
                await UpdateStatus(getUnreconciledDocumentAmount, reconciledTotalDocAmount, entity.DocumentTransactionId);

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

        private async Task UpdateStatus(RecordLedger ledger, decimal reconciledTotalAmount, int transactionId)
        {
            //FOR UPDATE STATUS
            if (ledger.Amount == reconciledTotalAmount)
            {
                var updateLedger = await _unitOfWork.Ledger.GetById(ledger.Id);
                updateLedger.setStatus(DocumentStatus.Reconciled);
                switch (ledger.Transactions.DocType)
                {
                    case DocType.Payment:
                        var payment = _unitOfWork.Payment.Find(new PaymentSpecs(transactionId)).FirstOrDefault();
                        payment.setStatus(5); // Paid
                        break;
                    case DocType.Receipt:
                        var receipt = _unitOfWork.Payment.Find(new PaymentSpecs(transactionId)).FirstOrDefault();
                        receipt.setStatus(5); // Paid
                        break;
                    case DocType.PayrollPayment:
                        var payrollPayment = _unitOfWork.Payment.Find(new PaymentSpecs(transactionId)).FirstOrDefault();
                        payrollPayment.setStatus(5); // Paid
                        break;
                    case DocType.CreditNote:
                        var creaditNote = _unitOfWork.CreditNote.Find(new CreditNoteSpecs(transactionId)).FirstOrDefault();
                        creaditNote.setStatus(5); // Paid
                        break;
                    case DocType.DebitNote:
                        var debitNote = _unitOfWork.DebitNote.Find(new DebitNoteSpecs(transactionId)).FirstOrDefault();
                        debitNote.setStatus(5); // Paid
                        break;
                    case DocType.Invoice:
                        var invoice = _unitOfWork.Invoice.Find(new InvoiceSpecs(transactionId)).FirstOrDefault();
                        invoice.setStatus(5); // Paid
                        break;
                    case DocType.Bill:
                        var bill = _unitOfWork.Bill.Find(new BillSpecs(transactionId)).FirstOrDefault();
                        bill.setStatus(5); // Paid
                        break;
                    case DocType.PayrollTransaction:
                        var payrollTransaction = _unitOfWork.PayrollTransaction.Find(new PayrollTransactionSpecs(transactionId)).FirstOrDefault();
                        payrollTransaction.setStatus(5); // Paid
                        break;
                }
            }
            else
            {
                var updateLedger = await _unitOfWork.Ledger.GetById(ledger.Id);
                updateLedger.setStatus(DocumentStatus.Partial);
                switch (ledger.Transactions.DocType)
                {
                    case DocType.Payment:
                        var payment = _unitOfWork.Payment.Find(new PaymentSpecs(transactionId)).FirstOrDefault();
                        payment.setStatus(4); // Partial
                        break;
                    case DocType.Receipt:
                        var receipt = _unitOfWork.Payment.Find(new PaymentSpecs(transactionId)).FirstOrDefault();
                        receipt.setStatus(4); // Partial
                        break;
                    case DocType.PayrollPayment:
                        var payrollPayment = _unitOfWork.Payment.Find(new PaymentSpecs(transactionId)).FirstOrDefault();
                        payrollPayment.setStatus(4); // Partial
                        break;
                    case DocType.CreditNote:
                        var creaditNote = _unitOfWork.CreditNote.Find(new CreditNoteSpecs(transactionId)).FirstOrDefault();
                        creaditNote.setStatus(4); // Partial
                        break;
                    case DocType.DebitNote:
                        var debitNote = _unitOfWork.DebitNote.Find(new DebitNoteSpecs(transactionId)).FirstOrDefault();
                        debitNote.setStatus(4); // Partial
                        break;
                    case DocType.Invoice:
                        var invoice = _unitOfWork.Invoice.Find(new InvoiceSpecs(transactionId)).FirstOrDefault();
                        invoice.setStatus(4); // Partial
                        break;
                    case DocType.Bill:
                        var bill = _unitOfWork.Bill.Find(new BillSpecs(transactionId)).FirstOrDefault();
                        bill.setStatus(4); // Partial
                        break;
                    case DocType.PayrollTransaction:
                        var payrollTransaction = _unitOfWork.PayrollTransaction.Find(new PayrollTransactionSpecs(transactionId)).FirstOrDefault();
                        payrollTransaction.setStatus(4); // Partial
                        break;
                }
            }
        }

        //Will do it later
        private Response<List<PaymentAmountListDto>> GetPaymentAmountListDto(int paymentTransactionId, int documentTransactionId, decimal amount)
        {
            //Checking if both id are same
            if (documentTransactionId == paymentTransactionId)
                return new Response<List<PaymentAmountListDto>>("Both transaction id cannot be same");

            //Getting transaction with Payment Transaction Id
            var getUnreconciledDocumentAmount = _unitOfWork.Ledger.Find(new LedgerSpecs(documentTransactionId)).FirstOrDefault();
            if (getUnreconciledDocumentAmount == null)
                return new Response<List<PaymentAmountListDto>>("No Transaction found for the given document transaction id");

            // Checking if given amount is greater than unreconciled document amount
            var reconciledDocumentAmount = _unitOfWork.TransactionReconcile.Find(new TransactionReconSpecs(getUnreconciledDocumentAmount.Id, false)).Sum(p => p.Amount);
            var unreconciledDocumentAmount = getUnreconciledDocumentAmount.Amount - reconciledDocumentAmount;
            if (amount > unreconciledDocumentAmount)
                return new Response<List<PaymentAmountListDto>>("Enter amount is greater than pending document amount");


            //Getting transaction with Document Transaction Id
            var getUnreconciledPaymentAmount = _unitOfWork.Ledger.Find(new LedgerSpecs(paymentTransactionId,
                getUnreconciledDocumentAmount.Level4_id, getUnreconciledDocumentAmount.BusinessPartnerId, getUnreconciledDocumentAmount.Sign)).ToList();
            if (!getUnreconciledPaymentAmount.Any())
                return new Response<List<PaymentAmountListDto>>("No Transaction found for the given payment transaction id");

            // declaring list for unconciledPaymentAmounts
            var unreconciledPaymentAmountList = new List<PaymentAmountListDto>();
            decimal unreconciledPaymentAmountTotal = 0;

            // looping thourgh unreconciled amount for adding them to list
            foreach (var item in getUnreconciledPaymentAmount)
            {
                var reconciledPaymentAmount = _unitOfWork.TransactionReconcile.Find(new TransactionReconSpecs(item.Id, true)).Sum(p => p.Amount);
                unreconciledPaymentAmountTotal += item.Amount - reconciledPaymentAmount;

                unreconciledPaymentAmountList.Add(new PaymentAmountListDto()
                {
                    LedgerId = item.Id,
                    UnreconciledAmount = item.Amount - reconciledPaymentAmount,
                });
            }
            return new Response<List<PaymentAmountListDto>>(unreconciledPaymentAmountList, "Returning List");
        }

        public Response<List<AmountsForReconciliationDto>> GetPaymentReconAmounts(Guid accountId, int businessPartnerId, char sign)
        {
            var amountsForRecociliationList = new List<AmountsForReconciliationDto>();
            var getUnreconciledPaymentAmountList = _unitOfWork.Ledger.Find(new LedgerSpecs(accountId, businessPartnerId, sign))
                .GroupBy(i => i.TransactionId)
                .Select(i => new { TransactionId = i.Key })
                .Select(i => i.TransactionId)
                .ToList();

            if (getUnreconciledPaymentAmountList.Any())
            {
                foreach (var trasactionId in getUnreconciledPaymentAmountList)
                {
                    //Getting transaction with Document Transaction Id
                    var getUnreconciledPaymentAmount = _unitOfWork.Ledger.Find(new LedgerSpecs(trasactionId,
                        accountId, businessPartnerId, sign, true)).ToList();
                    if (getUnreconciledPaymentAmount.Any())
                    {
                        decimal RecociledPaymentAmountTotal = 0;
                        decimal unreconciledPaymentAmountTotal = 0;

                        // looping thourgh unreconciled amount for adding them to list
                        foreach (var item in getUnreconciledPaymentAmount)
                        {
                            var reconciledPaymentAmount = _unitOfWork.TransactionReconcile.Find(new TransactionReconSpecs(item.Id, true)).Sum(p => p.Amount);
                            RecociledPaymentAmountTotal += reconciledPaymentAmount;
                            unreconciledPaymentAmountTotal += item.Amount - reconciledPaymentAmount;
                        }

                        var docDetails = getUnreconciledPaymentAmount.FirstOrDefault();
                        string[] docId = docDetails.Transactions.DocNo.Split("-");
                        amountsForRecociliationList.Add(new AmountsForReconciliationDto
                        {
                            DocumentId = Int32.Parse(docId[1]),
                            PaymentTransactionId = docDetails.TransactionId,
                            DocNo = docDetails.Transactions.DocNo,
                            DocType = docDetails.Transactions.DocType,
                            TotalAmount = getUnreconciledPaymentAmount.Sum(i => i.Amount),
                            ReconciledAmount = RecociledPaymentAmountTotal,
                            UnreconciledAmount = unreconciledPaymentAmountTotal
                        });
                    }
                }
            }
            return new Response<List<AmountsForReconciliationDto>>(amountsForRecociliationList, "Return amounts");
        }
    }
}
