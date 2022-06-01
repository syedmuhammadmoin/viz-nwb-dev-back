using Application.Contracts.Filters;
using Domain.Constants;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class PaymentSpecs : BaseSpecification<Payment>
    {
        public PaymentSpecs(PaginationFilter filter, DocType docType) : base(e => (e.PaymentFormType == docType))
        {
            var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
            ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
            ApplyOrderByDescending(i => i.Id);
            AddInclude(i => i.BusinessPartner);
            AddInclude(i => i.Account);
            AddInclude(i => i.Campus);
            AddInclude(i => i.Status);
            AddInclude(i => i.PaymentRegister);
        }
        public PaymentSpecs()
        {
            AddInclude(i => i.Status);
        }

        public PaymentSpecs(bool forEdit, DocType docType) : base(e => (e.PaymentFormType == docType))
        {
            if (!forEdit)
            {
                AddInclude(i => i.BusinessPartner);
                AddInclude(i => i.Account);
                AddInclude(i => i.Campus);
                AddInclude(i => i.Status);
                AddInclude(i => i.PaymentRegister);
            }
        }
        public PaymentSpecs(bool forRecon) : base(p => p.BankReconStatus != DocumentStatus.Reconciled)
        {
        }
        public PaymentSpecs(int transactionId) :
            base(p => (p.Status.State == DocumentStatus.Unpaid
            || p.Status.State == DocumentStatus.Partial) && (p.TransactionId == transactionId))
        {

        }
        public PaymentSpecs(string forWorkFlow) : base(e => (e.Status.State != DocumentStatus.Unpaid && e.Status.State != DocumentStatus.Partial && e.Status.State != DocumentStatus.Paid && e.Status.State != DocumentStatus.Draft && e.Status.State != DocumentStatus.Cancelled))
        {
            AddInclude(i => i.Status);
        }
        public PaymentSpecs(PaymentType paymentType, DocType docType) : base(e => (e.PaymentType == paymentType) && (e.PaymentFormType == docType))
        {
        }

        public PaymentSpecs(Guid paymentRegisterId) : base(
            (x =>
            (x.PaymentRegisterId == paymentRegisterId)
            && (x.BankReconStatus == DocumentStatus.Unreconciled || x.BankReconStatus == DocumentStatus.Partial)
            ))
        {
        }
        public PaymentSpecs(DocType docType, bool isApproval) 
            : base(e => isApproval ?
            ((e.PaymentFormType == docType)
            && (e.TransactionId != 0 || e.TransactionId != null)
            && (e.Status.State == DocumentStatus.Submitted || e.Status.State == DocumentStatus.Reviewed))
            :
            ((e.PaymentFormType == docType) 
            && (e.TransactionId != 0 || e.TransactionId != null) 
            &&(e.Status.State == DocumentStatus.Draft || e.Status.State == DocumentStatus.Rejected)))
        {
            {
                AddInclude(i => i.BusinessPartner);
                AddInclude(i => i.Account);
                AddInclude(i => i.Campus);
                AddInclude(i => i.Status);
                AddInclude(i => i.PaymentRegister);
            }
        }
        public PaymentSpecs(DocType docType) : base(e => ((e.PaymentFormType == docType) && (e.TransactionId != 0 || e.TransactionId != null) && (e.Status.State == DocumentStatus.Draft || e.Status.State == DocumentStatus.Rejected)))
        {
            {
                AddInclude(i => i.BusinessPartner);
                AddInclude(i => i.Account);
                AddInclude(i => i.Campus);
                AddInclude(i => i.Status);
                AddInclude(i => i.PaymentRegister);
            }
        }
    }
}
