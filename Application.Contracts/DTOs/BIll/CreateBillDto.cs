using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreateBillDto
    {
        public int? Id { get; set; }
        [Required]
        public int? VendorId { get; set; }
        [Required]
        public DateTime? BillDate { get; set; }
        [Required]
        public DateTime? DueDate { get; set; }
        [MaxLength(20)]
        public string Contact { get; set; }        
        public int? CampusId { get; set; }
        public int? GRNId { get; set; }
        [Required]
        public bool? IsSubmit { get; set; }
        [Required]
        public virtual List<CreateBillLinesDto> BillLines { get; set; }
    }
}
