using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class JournalEntryLinesDto 
    {
        public int Id { get; set; }
        public Guid AccountId { get; set; }
        public string AccountName { get; set; }
        public int BusinessPartnerId { get; set; }
        public string BusinessPartnerName { get; set; }
        public string Description { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public int MasterId { get; set; }
    }
}
