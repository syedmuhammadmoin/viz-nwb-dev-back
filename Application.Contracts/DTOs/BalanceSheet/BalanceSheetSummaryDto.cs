using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class BalanceSheetSummaryDto
    {
        public string Level1Id { get; set; }
        public string Level2Id { get; set; }
        public string Level3Id { get; set; }
        public string Level1Name { get; set; } 
        public string Level2Name { get; set; } 
        public string Level3Name { get; set; } 
        public decimal Balance { get; set; }
        
    }
}
