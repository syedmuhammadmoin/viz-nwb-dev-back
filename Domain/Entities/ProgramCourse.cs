using Domain.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class ProgramCourse : BaseEntity<int>
    {
        public int ProgramId { get; private set; }
        [ForeignKey("ProgramId")]
        public Program Program { get; private set; }

        public int SemesterId { get; private set; }
        [ForeignKey("SemesterId")]
        public Semester Semester { get; private set; }

        public int CourseId { get; private set; }
        [ForeignKey("CourseId")]
        public Course Course { get; private set; }

        protected ProgramCourse()
        {
        }

        public ProgramCourse(int programId, int semesterId, int courseId)
        {
            ProgramId = programId;
            SemesterId = semesterId;
            CourseId = courseId;
        }
    }
}
