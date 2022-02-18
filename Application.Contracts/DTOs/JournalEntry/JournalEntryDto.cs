using Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class JournalEntryDto
    {
        public int Id { get; set; }
        public string DocNo { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public DocumentStatus Status { get; set; }
        public decimal TotalDebit { get; set; }
        public decimal TotalCredit { get; set; }
        public virtual List<JournalEntryLinesDto> JournalEntryLines { get; set; }
    }
}
