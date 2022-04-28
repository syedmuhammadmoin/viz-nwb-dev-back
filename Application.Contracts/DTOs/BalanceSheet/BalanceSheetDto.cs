using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class BalanceSheetDto
    {
        public string Nature { get; set; } // Level1 Name
        public string Transactional { get; set; } // Level4 Name
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal Balance { get; set; }
        public DateTime DocDate { get; set; }
    }
}
