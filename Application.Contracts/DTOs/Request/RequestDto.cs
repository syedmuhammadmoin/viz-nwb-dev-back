using Domain.Constants;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class RequestDto
    {
        public int Id { get; set; }
        public string DocNo { get; set; }
        public DateTime RequestDate { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Campus { get; set; }
        public int CampusId { get; set; }
        public DocumentStatus State { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public virtual List<RequestLinesDto> RequestLines { get; set; }

        public IEnumerable<ReferncesDto> References { get; set; }
        public IEnumerable<RemarksDto> RemarksList { get; set; }
        public bool IsAllowedRole { get; set; }
    }
}
