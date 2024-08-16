using Domain.Base;
using Domain.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class CWIP : BaseEntity<int>
    {
        [MaxLength(20)]
        public string CwipCode { get; private set; }
        public DateTime DateOfAcquisition { get; private set; }
        [MaxLength(200)]
        public string Name { get; private set; }

        public string CWIPAccountId { get; private set; }
        [ForeignKey("CWIPAccountId")]
        public Level4 CWIPAccount { get; private set; }

        public int Cost { get; private set; }

        public int ProductId { get; private set; }
        [ForeignKey("ProductId")]
        public Product Product { get; private set; }

        public int WarehouseId { get; private set; }
        [ForeignKey("WarehouseId")]
        public Warehouse Warehouse { get; private set; }
        
        public int? SalvageValue { get; private set; }
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

        public int Quantity { get; private set; }
        public bool ProrataBasis { get; private set; }
        public bool IsActive { get; private set; }

        public int StatusId { get; private set; }
        [ForeignKey("StatusId")]
        public WorkFlowStatus Status { get; private set; }
        public int? TransactionId { get; private set; }
        [ForeignKey("TransactionId")]
        public Transactions Transactions { get; private set; }

        protected CWIP()
        {
        }

        public void SetStatus(int statusId)
        {
            StatusId = statusId;
        }
        public void SetTransactionId(int transactionId)
        {
            TransactionId = transactionId;
        }

        public void CreateCode()
        {
            //Creating doc no..
            CwipCode = "CWIP-" + String.Format("{0:000}", Id);
        }

    }
}
