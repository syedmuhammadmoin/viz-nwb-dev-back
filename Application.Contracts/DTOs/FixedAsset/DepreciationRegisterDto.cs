using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Constants;

namespace Application.Contracts.DTOs
{
    public class DepreciationRegisterDto
    {
        public int FixedAssetId { get; set; }
        public DateTime TransectionDate { get; set; }
        public bool IsAutomatedCalculation { get; set; }
        public decimal DepreciationAmount { get; set; }
        public string Description { get; set; }
    }
}
