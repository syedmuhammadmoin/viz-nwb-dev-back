using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreatePettyCashDto
    {
        public int? Id { get; set; }
        [Required]
        public DateTime? Date { get; set; }
        [Required]
        public Guid? AccountId { get; set; }
        public decimal? OpeningBalance { get; set; }
        public decimal? ClosingBalance { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }        
        public int? CampusId { get; set; }
        [Required]
        public bool? isSubmit { get; set; }
        [Required]
        public virtual List<CreatePettyCashLinesDto> PettyCashLines { get; set; }
    }
}
