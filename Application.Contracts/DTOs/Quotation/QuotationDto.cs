using Domain.Constants;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class QuotationDto
    {
        public int Id { get; set; }
        public string DocNo { get; set; }
        public DateTime QuotationDate { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public string Timeframe { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public DocumentStatus State { get; set; }
        public bool IsAllowedRole { get; set; }
        public int? RequisitionId { get; set; }
        public virtual List<QuotationLinesDto> QuotationLines { get; set; }

        public IEnumerable<RemarksDto> RemarksList { get; set; }
        public IEnumerable<ReferncesDto> References { get; set; }
        public IEnumerable<FileUploadDto> FileUploadList { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string LastUser
        {
            get { return RemarksList?.LastOrDefault().UserName ?? ModifiedBy ?? CreatedBy; }
        }
    }
}
