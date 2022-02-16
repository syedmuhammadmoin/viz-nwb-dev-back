using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class JournalEntryMaster : BaseEntity<int>
    {
        [MaxLength(20)]
        public string DocNo { get; private set; }
        public DateTime Date { get; private set; }
        [MaxLength(500)]
        public string Description { get; private set; }
        public List<JournalEntryLines> JournalEntryLines { get; set; }
        public JournalEntryMaster(JournalEntryMaster journalEntry)
        {
            DocNo = journalEntry.DocNo;
            Date = journalEntry.Date;
            Description = journalEntry.Description;
            JournalEntryLines = journalEntry.JournalEntryLines;
        }
        protected JournalEntryMaster()
        {
        }
    }
}
