using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreateIssuanceDto
    {
        public int? Id { get; set; }
        [Required]
        public int EmployeeId { get; set; }
        [Required]
        public DateTime IssuanceDate { get; set; }
        [Required]
        public int CampusId { get; set; }
        public int RequisitionId { get; set; }
        [Required]
        public bool isSubmit { get; set; }
        public virtual List<CreateIssuanceLinesDto> IssuanceLines { get; set; }
    }
}
