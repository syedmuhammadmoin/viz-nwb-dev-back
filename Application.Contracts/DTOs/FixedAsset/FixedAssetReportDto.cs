using Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class FixedAssetReportDto
    {
        public string Month { get; set; }
        public int? FixedAssetId { get; set; }
        public string FixedAsset { get; set; }
        public int? StoreId { get; set; }
        public string Store { get; set; }
        public string Category { get; set; }
        public int? UsefullLife { get; set; }
        public decimal UnitCost { get; set; }
        public decimal DepRate { get; set; }
        public decimal OpeningAccDep { get; set; }
        public decimal DepChargedForThePeriod { get; set; }
        public decimal ClosingAccDep { get; set; }
        public decimal NBV { get; set; }
    }
}
