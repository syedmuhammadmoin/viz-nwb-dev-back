using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreateCreditNoteDto
    {
        public int? Id { get; set; }
        [Required]
        public int? CustomerId { get; set; }
        [Required]
        public DateTime? NoteDate { get; set; }
        [Required]
        public int? CampusId { get; set; }
        [Required]
        public bool? isSubmit { get; set; }
        public int? DocumentLedgerId { get; set; }
        public virtual List<CreateCreditNoteLinesDto> CreditNoteLines { get; set; }
    }
}
