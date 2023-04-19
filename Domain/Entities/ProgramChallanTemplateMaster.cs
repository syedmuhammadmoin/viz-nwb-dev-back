using Domain.Base;
using Domain.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class ProgramChallanTemplateMaster : BaseEntity<int>
    {
        public ProgramChallanType ProgramChallanTypeMyProperty { get; private set; }

        public int ProgramId { get; private set; }
        [ForeignKey("ProgramId")]
        public Program Program { get; private set; }

        public int CampusId { get; private set; }
        [ForeignKey("CampusId")]
        public Campus Campus { get; private set; }

        public int ShiftId { get; private set; }
        [ForeignKey("ShiftId")]
        public Shift Shift { get; private set; }

        public int? SemesterId { get; private set; }
        [ForeignKey("SemesterId")]
        public Semester Semester { get; private set; }

        public int? ExamId { get; private set; }

        public Guid BankAccountId { get; private set; }
        [ForeignKey("BankAccountId")]
        public Level4 BankAccount { get; private set; }

        [MaxLength(300)]
        public string Description { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal LateFeeAfterDueDate { get; private set; }
        public DateTime ChallanDate { get; private set; }
        public DateTime DueDate { get; private set; }
        public virtual List<ProgramChallanTemplateLines> ProgramChallanTemplateLines { get; private set; }

        protected ProgramChallanTemplateMaster()
        {
        }

    }
}
