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
        public DateTime NoteDate { get; set; }
        public DocumentStatus Status { get; set; }
        public decimal TotalBeforeTax { get; set; }
        public decimal TotalTax { get; set; }
        public decimal TotalAmount { get; set; }
        public virtual List<CreditNoteLinesDto> CreditNoteLines { get; set; }
    }
}
