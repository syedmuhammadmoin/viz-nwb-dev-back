using Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CallForQuotationDto
    {
        public int Id { get; set; }
        public string DocNo { get; set; }
        public DateTime CallForQuotationDate { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public string Description { get;  set; }
        public string Status { get; set; }
        public DocumentStatus State { get; set; }
        public bool IsAllowedRole { get; set; }
        public virtual List<CallForQuotationLinesDto> CallForQuotationLines { get; set; }
        public IEnumerable<FileUploadDto> FileUploadList { get; set; }

    }
}
