using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreateGRNDto
    {
        public int? Id { get; set; }
        [Required]
        public int VendorId { get; set; }
        [Required]
        public DateTime GrnDate { get; set; }
        [StringLength(20)]
        public string Contact { get; set; }
        [Required]
        public int CampusId { get; set; }
        [Required]
        public bool isSubmit { get; set; }
        public virtual List<CreateGRNLinesDto> GRNLines { get; set; }
    }
}
