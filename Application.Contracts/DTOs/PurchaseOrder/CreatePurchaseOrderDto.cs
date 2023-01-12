using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreatePurchaseOrderDto
    {
        public int? Id { get; set; }
        [Required]
        public int? VendorId { get; set; }
        [Required]
        public DateTime? PODate { get; set; }
        [Required]
        public DateTime? DueDate { get; set; }
        [StringLength(20)]
        public string Contact { get; set; }
        [Required]
        public int? CampusId { get; set; }
        [Required]
        public bool? isSubmit { get; set; }
        public virtual List<CreatePurchaseOrderLinesDto> PurchaseOrderLines { get; set; }
        public int? RequisitionId { get; private set; }

    }
}
