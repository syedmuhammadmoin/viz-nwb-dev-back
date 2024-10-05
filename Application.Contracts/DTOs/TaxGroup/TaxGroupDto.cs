using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs.TaxGroup
{
    public class TaxGroupDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CountryId { get; set; }
        public string Company { get; set; }
        public string CountryName { get; set; }
        public int Sequence { get; set; }
        public string PayableAccountId { get; set; }
        public string PayableAccountName { get; set; }
        public string ReceivableAccountId { get; set; }
        public string ReceivableAccountName { get; set; }
        public string AdvanceAccountId { get; set; }
        public string AdvanceAccountName { get; set; }
        public decimal PreceedingTtl { get; set; }
    }
}
