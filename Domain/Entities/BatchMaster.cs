using Domain.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class BatchMaster : BaseEntity<int>
    {
        [MaxLength(200)]
        public string Name { get; private set; }
        public DateTime StartDate { get; private set; }
        
        public int SemesterId { get; private set; }
        [ForeignKey("SemesterId")]
        public Semester Semester { get; private set; }
        
        public int CampusId { get; private set; }
        [ForeignKey("CampusId")]
        public Campus Campus { get; private set; }

        public int ShiftId { get; private set; }
        [ForeignKey("ShiftId")]
        public Shift Shift { get; private set; }

        public bool IsAdmissionOpen { get; private set; }
        public virtual List<BatchLines> BatchLines { get; private set; }
        public virtual List<BatchAdmissionCriteria> Criteria { get; private set; }

        protected BatchMaster()
        {
        }
    }
}
