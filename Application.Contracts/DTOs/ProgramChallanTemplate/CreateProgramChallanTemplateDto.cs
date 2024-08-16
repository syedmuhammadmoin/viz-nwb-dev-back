using Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace Application.Contracts.DTOs
{
    public class CreateProgramChallanTemplateDto
    {
        public int? Id { get; set; }
        [Required]
        public ProgramChallanType? ProgramChallanTypeMyProperty { get; set; }
        [Required]
        public int? ProgramId { get; set; }
        [Required] 
        public int? CampusId { get; set; }
        [Required] 
        public int? ShiftId { get; set; }
        public int? SemesterId { get; set; }
        public int? ExamId { get; set; }
        [Required]
        public string BankAccountId { get; set; }
        [Required]
        [MaxLength(300)]
        public string Description { get; set; }
        [Required]
        [Range(0.00, int.MaxValue, ErrorMessage = "Value can't be negative")]
        public decimal? LateFeeAfterDueDate { get; set; }
        [Required]
        public DateTime ChallanDate { get; set; }
        [Required]
        public DateTime DueDate { get; set; }
        [Required]
        public virtual List<CreateProgramChallanTemplateLinesDto> ProgramChallanTemplateLines { get; set; }

    }
}
