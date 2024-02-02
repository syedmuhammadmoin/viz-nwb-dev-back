using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class AggregatedRecordLedgerDto
    {
        public Guid Level4Id { get; set; }
        public Guid Level1Id { get; set; }
        public string Level4Name { get; set; }
        public string Level1Name { get; set; }
        public decimal Balance { get; set; }

    }
}
