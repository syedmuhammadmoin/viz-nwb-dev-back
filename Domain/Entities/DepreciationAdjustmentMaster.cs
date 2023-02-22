using Domain.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class DepreciationAdjustmentMaster : BaseEntity<int>
    {
        [MaxLength(20)]
        public string DocNo { get; private set; }
        public DateTime DateOfDepreciationAdjustment { get; private set; }
        [MaxLength(500)]
        public string Description { get; private set; }
        public int StatusId { get; private set; }
        [ForeignKey("StatusId")]
        public WorkFlowStatus Status { get; private set; }
        public int? TransactionId { get; private set; }
        [ForeignKey("TransactionId")]
        public Transactions Transactions { get; private set; }
        public virtual List<DepreciationAdjustmentLines> DepreciationAdjustmentLines { get; private set; }

        protected DepreciationAdjustmentMaster()
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

        public void CreateDocNo()
        {
            //Creating doc no..
            DocNo = "DEPADJ-" + String.Format("{0:000}", Id);
        }
    }
}
