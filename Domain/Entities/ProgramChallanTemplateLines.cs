using Domain.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class ProgramChallanTemplateLines : BaseEntity<int>
    {
        public int FeeItemId { get; private set; }
        [ForeignKey("FeeItemId")]
        public FeeItem FeeItem { get; private set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; private set; }
        
        public int MasterId { get; private set; }
        [ForeignKey("MasterId")]
        public ProgramChallanTemplateMaster ProgramChallanTemplateMaster { get; private set; }

        protected ProgramChallanTemplateLines()
        {
        }
    }
}
