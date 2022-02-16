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
    public class JournalEntryLines : BaseEntity<int>
    {
        public Guid AccountId { get; set; }
        [ForeignKey("AccountId")]
        public Level4 Account { get; set; }
        public int? BusinessPartnerId { get; set; }
        [ForeignKey("BusinessPartnerId")]
        public BusinessPartner BusinessPartner { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Debit { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Credit { get; set; }
        public int LocationId { get; set; }
        [ForeignKey("LocationId")]
        public Location Location { get; set; }
        public int MasterId { get; set; }
        [ForeignKey("MasterId ")]
        public JournalEntryMaster JournalEntryMaster { get; set; }

        public JournalEntryLines(JournalEntryLines journalEntryLines)
        {
            AccountId = journalEntryLines.AccountId;
            BusinessPartnerId = journalEntryLines.BusinessPartnerId;
            Description = journalEntryLines.Description;
            Debit = journalEntryLines.Debit;
            Credit = journalEntryLines.Credit;
            LocationId = journalEntryLines.LocationId;
        }
        protected JournalEntryLines()
        {
        }


    }
}
