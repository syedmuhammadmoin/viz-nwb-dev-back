using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class PNLSummaryDTO
    {
        public int Month { get; set; }
        public int year { get; set; }
        public Guid Level1Id { get; set; }
        public string Nature { get; set; }
        public decimal Balance { get; set; }
    }
}
