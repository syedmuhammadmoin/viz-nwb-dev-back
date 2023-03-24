using Domain.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class ProgramFees : BaseEntity<int>
    {
        public int ProgramSemesterId { get; private set; }
        [ForeignKey("ProgramSemesterId")]
        public ProgramSemester ProgramSemester { get; private set; }

        public int FeeItemId { get; private set; }
        [ForeignKey("FeeItemId")]
        public FeeItem FeeItem { get; private set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; private set; }

        protected ProgramFees()
        {
        }

        public ProgramFees(int programSemesterId, int feeItemId, decimal amount)
        {
            ProgramSemesterId = programSemesterId;
            FeeItemId = feeItemId;
            Amount = amount;
        }

    }
}
