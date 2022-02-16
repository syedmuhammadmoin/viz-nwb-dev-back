using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class JournalEntryDto
    {
        public decimal TotalDebit { get; set; }
        public decimal TotalCredit { get; set; }
        public virtual IEnumerable<JournalEntryLinesDto> JournalEntryLines { get; set; }
        public bool IsAllowedRole { get; set; }
    }
}
