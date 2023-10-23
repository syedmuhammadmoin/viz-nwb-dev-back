using Domain.Constants;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Contracts.DTOs
{
    public class AdmissionApplicationDto
    {
        public string DocNo { get; set; }
        public int ApplicantId { get; set; }
        public ApplicantDto Applicant { get; set; }
        public int? StudentId { get; set; }
        public int BatchId { get; set; }
        public string Batch { get; set; }
        public int ProgramId { get; set; }
        public string Program { get; set; }
        public int CampusId { get; set; }
        public string Campus { get; set; }
        public int ShiftId { get; set; }
        public string Shift { get; set; }
        public string Remarks { get; set; }
        public int? AdmissionCriteriaId { get; set; }
        public AdmissionCriteriaDto AdmissionCriteria { get; set; }
        public bool? IsEntryTestRequired { get; private set; }
        public DateTime? EntryTestDate { get; private set; }
        public Attendance? EntryTestAttendance { get; private set; }
        public decimal? EntryTestMarks { get; private set; }
        public decimal? EntryTestRequriedMarks { get; private set; }
        public DateTime? InterviewDate { get; private set; }
        public bool? IsInterviewRequired { get; private set; }
        public Attendance? InterviewAttendance { get; private set; }
        public InterviewStatus? InterviewStatus { get; set; }
        public ApplicationStatus State { get; set; }
        public string Status { get; set; }
    }
}
