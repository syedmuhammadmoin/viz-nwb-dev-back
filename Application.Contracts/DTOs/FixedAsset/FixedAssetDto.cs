using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Constants;

namespace Application.Contracts.DTOs
{
    public class FixedAssetDto
    {
        public int Id { get; set; }
        public string AssetCode { get; set; }
        public DateTime DateofAcquisition { get; set; }
        public string Name { get; set; }
        public int PurchaseCost { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int SalvageValue { get; set; }
        public bool DepreciationApplicability { get; set; }
        public int? DepreciationModelId { get; set; }
        public string DepreciationModel { get; set; }
        public int CampusId { get; set; }
        public string Campus { get; set; }
        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public DepreciationMethod ModelType { get; set; }
        public Guid? AssetAccountId { get; set; }
        public string AssetAccount { get; set; }
        public Guid? DepreciationExpenseId { get; set; }
        public string DepreciationExpense { get; set; }
        public Guid? AccumulatedDepreciationId { get; set; }
        public int Quantity { get; set; }
        public string AccumulatedDepreciation { get; set; }
        public int UseFullLife { get; set; }
        public decimal DecLiningRate { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public DocumentStatus State { get; set; }
        public bool ProrataBasis { get; set; }
        public bool Active { get; set; }
        public IEnumerable<RemarksDto> RemarksList { get; set; }
        public bool IsAllowedRole { get; set; }
        public bool IsHeldforSaleOrDisposal { get; set; }
        public int Quantinty { get; set; }
        public string GRNDocNo { get; set; }


    }
}
