using Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class DebitNoteDto
    {
        public int Id { get; set; }
        public string DocNo { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public Guid PayableAccountId { get; set; }
        public string PayableAccountName { get; set; }
        public DateTime NoteDate { get; set; }
        public DocumentStatus Status { get; set; }
        public int CampusId { get; set; }
        public string CampusName { get; set; }
        public decimal TotalBeforeTax { get; set; }
        public decimal TotalTax { get; set; }
        public decimal TotalAmount { get; set; }
        public int TransactionId { get; set; }
        public virtual List<DebitNoteLinesDto> DebitNoteLines { get; set; }
    }
}
