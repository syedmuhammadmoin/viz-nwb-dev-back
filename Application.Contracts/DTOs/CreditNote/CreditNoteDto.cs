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
        public decimal UnreconciledAmount { get; set; }
        public PaidDocListDto DocumentReconcile { get; set; }
        public int? LedgerId { get; set; }
        public IEnumerable<RemarksDto> RemarksList { get; set; }
        public int? TransactionId { get; set; }
        public virtual List<CreditNoteLinesDto> CreditNoteLines { get; set; }
        public bool IsAllowedRole { get; set; }
    }
}
