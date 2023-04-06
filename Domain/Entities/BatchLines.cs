using Domain.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class BatchLines : BaseEntity<int>
    {
        public int ProgramId { get; private set; }
        [ForeignKey("ProgramId")]
        public Program Program { get; private set; }

        public int MasterId { get; private set; }
        [ForeignKey("MasterId")]
        public BatchMaster BatchMaster { get; private set; }

        protected BatchLines()
        {
        }
    }
}
