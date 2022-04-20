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
        public DateTime RequisitionDate { get; private set; }
        public string Contact { get; private set; }
        public int CampusId { get; private set; }
        public bool isSubmit { get; set; }
        public virtual List<CreatePurchaseOrderLinesDto> RequisitionLines { get; private set; }
    }
}
