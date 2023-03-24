using Domain.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class ProgramCourse : BaseEntity<int>
    {
        public int ProgramSemesterId { get; private set; }
        [ForeignKey("ProgramSemesterId")]
        public ProgramSemester ProgramSemester { get; private set; }
        
        public int CourseId { get; private set; }
        [ForeignKey("CourseId")]
        public Course Course { get; private set; }

        protected ProgramCourse()
        {
        }

        public ProgramCourse(int programSemesterId, int courseId)
        {
            ProgramSemesterId = programSemesterId;
            CourseId = courseId;
        }
    }
}
