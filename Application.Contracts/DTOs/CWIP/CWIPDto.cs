using Domain.Constants;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CWIPDto
    {
        public int Id { get; set; }
        public DateTime DateOfAcquisition { get; set; }
        public Guid CWIPAccountId { get; set; }
        public string CWIPAccount { get; set; }
        public int CostOfAsset { get; set; }
        public Guid AssetAccountId { get; set; }
        public string AssetAccount { get; set; }
        public int? SalvageValue { get; set; }
        public bool DepreciationApplicability { get; set; }
        public int? DepreciationId { get; set; }
        public string Depreciation { get; set; }
        public DepreciationMethod ModelType { get; set; }
        public Guid? DepreciationExpenseId { get; set; }
        public string DepreciationExpense { get; set; }
        public Guid? AccumulatedDepreciationId { get; set; }
        public string AccumulatedDepreciation { get; set; }
        public int UseFullLife { get; set; }
        public int Quantinty { get; set; }
        public decimal? DecLiningRate { get; set; }
        public bool ProrataBasis { get; set; }
        public bool Active { get; set; }
    }
}
