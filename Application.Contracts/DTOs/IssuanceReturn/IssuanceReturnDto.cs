using Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class IssuanceReturnDto
    {
        public int Id { get; set; }
        public string DocNo { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public DateTime IssuanceReturnDate { get; set; }
        public int CampusId { get; set; }
        public string CampusName { get; set; }
        public string Contact { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public int IssuanceId { get; set; }
        public DocumentStatus State { get; set; }
        public string IssuanceDocNo { get; set; }
        public virtual List<IssuanceReturnLinesDto> IssuanceReturnLines { get; set; }
        public bool IsAllowedRole { get; set; }
    }
}
