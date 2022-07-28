﻿using Application.Contracts.DTOs.FileUpload;
using Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class GRNDto
    {
        public int Id { get; set; }
        public string DocNo { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public BusinessPartnerType Type { get; set; }
        public DateTime GrnDate { get; set; }
        public int CampusId { get; set; }
        public string CampusName { get; set; }
        public string Contact { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public int PurchaseOrderId { get; set; }
        public DocumentStatus State { get; set; }
        public decimal TotalBeforeTax { get; set; }
        public decimal TotalTax { get; set; }
        public decimal TotalAmount { get; set; }
        public string PODocNo { get; set; }
        public string IssuanceDocNo { get; set; }
        public IEnumerable<ReferncesDto> References { get; set; }
        public IEnumerable<RemarksDto> RemarksList { get; set; }
        public IEnumerable<FileUploadDto> FileUploadList { get; set; }

        public ReferncesDto BillReference { get; set; }
        public virtual List<GRNLinesDto> GRNLines { get; set; }
        public bool IsAllowedRole { get; set; }
    }
}
