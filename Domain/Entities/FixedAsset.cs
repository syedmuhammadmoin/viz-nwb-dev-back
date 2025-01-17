﻿using Domain.Base;
using Domain.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class FixedAsset : BaseEntity<int>
    {
        [MaxLength(20)]
        public string AssetCode { get; private set; }
        public DateTime DateofAcquisition { get; private set; }
        [MaxLength(200)]
        public string Name { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Cost { get; private set; }

        public int ProductId { get; private set; }
        [ForeignKey("ProductId")]
        public Product Product { get; private set; }

        public int WarehouseId { get; private set; }
        [ForeignKey("WarehouseId")]
        public Warehouse Warehouse { get; private set; }

        public int SalvageValue { get; private set; }
        public bool DepreciationApplicability { get; private set; }
        
        public int? DepreciationModelId { get; private set; }
        [ForeignKey("DepreciationModelId")]
        public DepreciationModel DepreciationModel { get; private set; }
        
        public int? UseFullLife { get; private set; }
        
        public string? AssetAccountId { get; private set; }
        [ForeignKey("AssetAccountId")]
        public Level4 AssetAccount { get; private set; }

        public string? DepreciationExpenseId { get; private set; }
        [ForeignKey("DepreciationExpenseId")]
        public Level4 DepreciationExpense { get; private set; }

        public string? AccumulatedDepreciationId { get; private set; }
        [ForeignKey("AccumulatedDepreciationId")]
        public Level4 AccumulatedDepreciation { get; private set; }

        public DepreciationMethod ModelType { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal DecLiningRate { get; private set; }

        public bool ProrataBasis { get; private set; }
        public bool IsActive { get; private set; }
        
        public int StatusId { get; private set; }
        [ForeignKey("StatusId")]
        public WorkFlowStatus Status { get; private set; }

        public bool IsHeldforSaleOrDisposal { get; private set; }
        public bool IsIssued { get; private set; }
        public bool IsReserved { get; private set; }
        public bool IsDisposed { get; private set; }

        public int? DocId { get; private set; }
        public DocType? Doctype { get; private set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal AccumulatedDepreciationAmount { get; private set; }
        public int TotalActiveDays { get; private set; }
        public int? EmployeeId { get; private set; }
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; private set; }
        public virtual List<FixedAssetLines> FixedAssetlines { get; set; }
        public virtual List<DepreciationRegister> DepreciatonRegisterList { get; set; }

        protected FixedAsset()
        {

        }
        
        public void SetStatus(int statusId)
        {
            StatusId = statusId;
        }

        public void SetHeldForDisposalTrue()
        {
            IsHeldforSaleOrDisposal = true;
            IsActive = false;
        }

        public void SetIsDisposedTrue()
        {
            IsDisposed = true;
        }

        public void SetIsReserved(bool isReserved)
        {
            IsReserved= isReserved;
        }
        public void SetIsIssued(bool isIssued)
        {
            IsIssued = isIssued;
        }
        public void SetEmployeeId(int? employeeId)
        {
            EmployeeId = employeeId;
        }
        public void CreateCode()
        {
            //Creating doc no..
            AssetCode = "FXA-" + String.Format("{0:000}", Id);
        }


        public void SetAccumulatedDepreciationAmount(decimal accumulatedDepreciationAmount)
        {
            AccumulatedDepreciationAmount = accumulatedDepreciationAmount;
        }
        public void SetTotalActiveDays(int totalActiveDays)
        {
            TotalActiveDays = totalActiveDays;
        }

    }
}
