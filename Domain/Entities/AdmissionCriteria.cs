using Domain.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class AdmissionCriteria : BaseEntity<int>
    {
        public int ProgramId { get; private set; }
        [ForeignKey("ProgramId")]
        public Program Program { get; private set; }
        
        [MaxLength(200)]
        public string Description { get; private set; }
        
        public int QualificationId { get; private set; }
        [ForeignKey("QualificationId")]
        public Qualification Qualification { get; private set; }
        
        public int SubjectId { get; private set; }
        [ForeignKey("SubjectId")]
        public Subject Subject { get; private set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal? QualificationRequriedMarks { get; private set; }
        public bool? IsEntryTestRequired { get; private set; }
        public DateTime? EntryTestDate { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? EntryTestRequriedMarks { get; private set; }
        public DateTime? InterviewDate { get; private set; }
        public bool? IsInterviewRequired { get; private set; }

        protected AdmissionCriteria()
        {
        }
    }
}
