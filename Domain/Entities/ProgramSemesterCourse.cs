using Domain.Base;
using Domain.Constants;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class ProgramSemesterCourse : BaseEntity<int>
    {
        public int ProgramId { get; private set; }
        [ForeignKey("ProgramId")]
        public Program Program { get; private set; }
        public SemesterNumber SemesterNumber { get; private set; }
        public int CourseId { get; private set; }
        [ForeignKey("CourseId")]
        public Course Course { get; private set; }

        protected ProgramSemesterCourse()
        {
        }
    }
}
