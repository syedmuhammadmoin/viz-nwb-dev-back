using Application.Contracts.DTOs.FileUpload;
using Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class PurchaseOrderDto
    {
        public int Id { get; set; }
        public string DocNo { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public DateTime PODate { get; set; }
        public DateTime DueDate { get; set; }
        public int CampusId { get; set; }
        public string CampusName { get; set; }
        public string Contact { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public DocumentStatus State { get; set; }
        public decimal TotalBeforeTax { get; set; }
        public decimal TotalTax { get; set; }
        public decimal TotalAmount { get; set; }
        public IEnumerable<ReferncesDto> References { get; set; }
        public virtual List<PurchaseOrderLinesDto> PurchaseOrderLines { get; set; }
        public IEnumerable <RemarksDto> RemarksList { get; set; }
        public IEnumerable <FileUploadDto> FileUploadList { get; set; }
        public bool IsAllowedRole { get; set; }
    }
}
