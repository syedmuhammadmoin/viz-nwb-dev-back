using Domain.Constants;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Application.Contracts.DTOs
{
    public class InvoiceDto
    {
        public int Id { get; set; }
        public string DocNo { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string IncomeTaxId { get; set; }
        public string SalesTaxId { get; set; }
        public Guid ReceivableAccountId { get; set; }
        public string ReceivableAccountName { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public string Contact { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public DocumentStatus State { get; set; }
        public int CampusId { get; set; }
        public string CampusName { get; set; }
        public decimal TotalBeforeTax { get; set; }
        public decimal TotalTax { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalPaid { get; set; }
        public IEnumerable<PaidDocListDto> PaidAmountList { get; set; }
        public IEnumerable<FileUploadDto> FileUploadList { get; set; }
        public IEnumerable<UnreconciledBusinessPartnerPaymentsDto> BPUnreconPaymentList { get; set; }
        public decimal PendingAmount { get; set; }
        public IEnumerable<RemarksDto> RemarksList { get; set; }
        public int? TransactionId { get; set; }
        public int? LedgerId { get; set; }
        public virtual List<InvoiceLinesDto> InvoiceLines { get; set; }
        public bool IsAllowedRole { get; set; }
    }
}
