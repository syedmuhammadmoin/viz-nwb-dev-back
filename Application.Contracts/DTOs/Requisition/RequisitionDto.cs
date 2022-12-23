using Application.Contracts.DTOs;
using Domain.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class RequisitionDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string DocNo { get; set; }
        public int StatusId { get; set; }
        public int? RequestId { get; private set; }

        public string Status { get; set; }
        public string Campus { get; set; }
        public int CampusId { get; set; }
        public DocumentStatus State { get; set; }
        public DateTime RequisitionDate { get; set; }
        public IEnumerable<ReferncesDto> References { get; set; }
        public IEnumerable<RemarksDto> RemarksList { get; set; }
        public virtual List<RequisitionLinesDto> RequisitionLines { get; set; }
        public IEnumerable<FileUploadDto> FileUploadList { get; set; }
        public bool IsAllowedRole { get; set; } = false;
        public bool IsWithoutWorkflow { get; set; } = false;
        public bool IsShowIssuanceButton { get; set; } = false;
        public bool IsShowPurchaseOrderButton { get; set; } = false;
        public bool IsShowCFQButton { get; set; } = false;
        public bool IsShowTenderButton { get; set; } = false;
    }
}
