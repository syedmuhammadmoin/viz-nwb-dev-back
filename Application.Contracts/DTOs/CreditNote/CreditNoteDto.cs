using Application.Contracts.DTOs;
using Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreditNoteDto
    {
        public int Id { get; set; }
        public string DocNo { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string IncomeTaxId { get; set; }
        public string SalesTaxId { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public DateTime NoteDate { get; set; }
        public Guid ReceivableAccountId { get;  set; }
        public string ReceivableAccountName { get;  set; }
        public int CampusId { get; set; }
        public string CampusName { get; set; }
        public DocumentStatus State { get; set; }
        public decimal TotalBeforeTax { get; set; }
        public decimal TotalTax { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal ReconciledAmount { get; set; }
        public IEnumerable<PaidDocListDto> PaidAmountList { get; set; }
        public IEnumerable<FileUploadDto> FileUploadList { get; set; }

        public decimal UnreconciledAmount { get; set; }
        public PaidDocListDto DocumentReconcile { get; set; }
        public int? LedgerId { get; set; }
        public IEnumerable<RemarksDto> RemarksList { get; set; }
        public int? TransactionId { get; set; }
        public virtual List<CreditNoteLinesDto> CreditNoteLines { get; set; }
        public bool IsAllowedRole { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string LastUser
        {
            get { return RemarksList?.LastOrDefault().UserName ?? ModifiedBy ?? CreatedBy; }
        }
    }
}
