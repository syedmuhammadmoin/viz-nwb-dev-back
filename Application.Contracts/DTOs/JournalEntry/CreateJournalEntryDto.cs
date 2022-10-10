using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreateJournalEntryDto
    {
        public int? Id { get; set; }
        public DateTime Date { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
        [Required]
        public int? CampusId { get; set; }
        [Required]
        public bool? isSubmit { get; set; }
        [Required]
        public virtual List<CreateJournalEntryLinesDto> JournalEntryLines { get; set; }
    }
}
