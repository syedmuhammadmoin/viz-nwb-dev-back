using Domain.Base;
using Domain.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class AdmissionApplication : BaseEntity<int>
    {
        [MaxLength(20)]
        public string DocNo { get; private set; }
        public int ApplicantId { get; private set; }
        [ForeignKey("ApplicantId")]
        public Applicant Applicant { get; private set; }

        public int? StudentId { get; private set; }

        public int BatchId { get; private set; }
        [ForeignKey("BatchId")]
        public BatchMaster Batch { get; private set; }

        public int ProgramId { get; private set; }
        [ForeignKey("ProgramId")]
        public Program Program { get; private set; }

        public int CampusId { get; private set; }
        [ForeignKey("CampusId")]
        public Campus Campus { get; private set; }

        public int ShiftId { get; private set; }
        [ForeignKey("ShiftId")]
        public Shift Shift { get; private set; }

        [MaxLength(200)]
        public string Remarks { get; private set; }

        public int? AdmissionCriteriaId { get; private set; }
        [ForeignKey("AdmissionCriteriaId")]
        public AdmissionCriteria AdmissionCriteria { get; private set; }

        public bool? IsEntryTestRequired { get; private set; }
        public DateTime? EntryTestDate { get; private set; }
        public Attendance? EntryTestAttendance { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? EntryTestMarks { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? EntryTestRequriedMarks { get; private set; }
        public DateTime? InterviewDate { get; private set; }
        public bool? IsInterviewRequired { get; private set; }
        public Attendance? InterviewAttendance { get; private set; }
        public InterviewStatus? InterviewStatus { get; set; }
        public ApplicationStatus Status { get; private set; }

        protected AdmissionApplication()
        {
        }

        public void CreateDocNo()
        {
            //Creating doc no..
            DocNo = "FormNo-" + String.Format("{0:000}", Id);
        }
    }
}
