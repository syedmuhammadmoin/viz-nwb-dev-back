using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreateRequisitionDto
    {
        public int? Id { get; set; }
        public int BusinessPartnerId { get; set; }
        public DateTime RequisitionDate { get; set; }
        public string Contact { get; set; }
        public int CampusId { get; set; }
        public bool isSubmit { get; set; }
        public virtual List<CreateRequisitionLinesDto> RequisitionLines { get; set; }
    }
}
