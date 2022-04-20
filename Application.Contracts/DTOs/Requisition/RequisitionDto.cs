using Application.Contracts.DTOs;
using Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class RequisitionDto
    {
        public int BusinessPartnerId { get; set; }
        public string BusinessPartner { get; set; }
        public string DocNo { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public string Campus { get; set; }
        public string CampusId { get; set; }
        public DocumentStatus State { get; set; }
        public DateTime RequisitionDate { get; set; }
        public virtual List<RequisitionLinesDto> RequisitionLines { get; set; }
        public bool IsAllowedRole { get; set; }
    }
}
