using Domain.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class ProgramSemester : BaseEntity<int>
    {
        public int ProgramId { get; private set; }
        [ForeignKey("ProgramId")]
        public Program Program { get; private set; }

        public int SemesterId { get; private set; }
        [ForeignKey("SemesterId")]
        public Semester Semester { get; private set; }

        protected ProgramSemester()
        {
        }

        public ProgramSemester(int programId, int semesterId)
        {
            ProgramId = programId;
            SemesterId = semesterId;
        }
    }
}
