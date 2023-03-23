using Domain.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class ProgramFees : BaseEntity<int>
    {
        public int ProgramId { get; private set; }
        [ForeignKey("ProgramId")]
        public Program Program { get; private set; }

        public int SemesterId { get; private set; }
        [ForeignKey("SemesterId")]
        public Semester Semester { get; private set; }

        public int FeeItemId { get; private set; }
        [ForeignKey("FeeItemId")]
        public FeeItem FeeItem { get; private set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; private set; }

        protected ProgramFees()
        {
        }
    }
}
